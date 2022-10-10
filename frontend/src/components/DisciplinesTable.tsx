import React, {FC, useState} from 'react'
import {Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@material-ui/core";
import {DragDropContext, Droppable, DropResult} from "react-beautiful-dnd";
import {tableRow} from "./tableRow";
import {Discipline} from "./Discipline";

interface tableProps {
    lecturersIds : string[]
    lecturers : {[key:string] : tableRow}
    disciplines : {[key:string] : string}
    
}

/// Represents a table with a pedagogical load with the possibility of Drag&Drop
export const DisciplinesTable : FC<tableProps> = ({lecturers, lecturersIds, disciplines} : tableProps) => {
    const [localTableData, setLocalTableData] = useState<tableProps>({lecturers, lecturersIds, disciplines})

    const onDragEnd = (result: DropResult) => {
        const destination = result.destination
        const source = result.source
        // the same place or drag to nowhere
        if ((!result.destination) || (result.destination.index === result.source.index &&
            result.destination.droppableId === result.source.droppableId)){
            return
        }  // the same lecturer
        else if (destination!.droppableId === source.droppableId){
            setLocalTableData(({lecturers, lecturersIds, disciplines}) => {
                const lecturer = lecturers[source.droppableId]
                const destinationIds = Array.from(lecturer.disciplineIds)
                destinationIds.splice(source.index,1)
                destinationIds.splice(destination!.index,0, result.draggableId)
                lecturers[source.droppableId].disciplineIds = destinationIds;
                return {lecturers, lecturersIds, disciplines}
            })
        }  // another lecturer
        else if (result.source.droppableId !== result.destination.droppableId) {
            setLocalTableData(({lecturers, lecturersIds, disciplines}) => {
                const sourceIds = Array.from(lecturers[source.droppableId].disciplineIds)
                sourceIds.splice(result.source.index, 1)
                const destinationIds = Array.from(lecturers[destination!.droppableId].disciplineIds)
                destinationIds.splice(destination!.index, 0, result.draggableId)
                lecturers[destination!.droppableId].disciplineIds = destinationIds;
                lecturers[source.droppableId].disciplineIds = sourceIds
                return {lecturers, lecturersIds, disciplines}
            })
        }

    }

    return(
        <TableContainer>
            <Table>
                <colgroup>
                    <col style={{width: "10%"}}/>
                    <col style={{width: "10%"}}/>
                    <col style={{width: "5%"}}/>
                    <col style={{width: "30%"}}/>
                    <col style={{width: "25%"}}/>
                    <col style={{width: "10%"}}/>
                </colgroup>
                <TableHead>
                    <TableRow>
                        <TableCell align={"left"}>ФИО</TableCell>
                        <TableCell align={"left"}>Должность</TableCell>
                        <TableCell align={"left"}>Процент ставки</TableCell>
                        <TableCell align={"left"}>Читаемые дисциплины</TableCell>
                        <TableCell align={"left"}>Распределённая нагрузка</TableCell>
                        <TableCell align={"left"}>Норматив</TableCell>
                    </TableRow>
                </TableHead>
                <DragDropContext onDragEnd={onDragEnd}>
                    <TableBody>
                    {localTableData.lecturersIds.map((lecturerId) => {
                        return <TableRow>
                            <TableCell align={"left"}>{localTableData.lecturers[lecturerId].name}</TableCell>
                            <TableCell align={"left"}>{localTableData.lecturers[lecturerId].post}</TableCell>
                            <TableCell align={"left"}>{localTableData.lecturers[lecturerId].interestRate}%</TableCell>
                            <Droppable droppableId={localTableData.lecturers[lecturerId].name}>
                                {(provided) => { return(
                                    <TableCell ref={provided.innerRef} >
                                            {localTableData.lecturers[lecturerId].disciplineIds.map((disciplineId, index) =>
                                            { return(
                                                <Discipline key={disciplineId} name={disciplineId} index={index} />
                                            )
                                            })}
                                        {provided.placeholder}
                                    </TableCell>)
                                }
                                }
                            </Droppable>
                            <TableCell align={"left"}>Распределённая нагрузка</TableCell>
                            <TableCell align={"left"}>Норматив</TableCell>
                        </TableRow>
                    })}
                    </TableBody>
                </DragDropContext>
            </Table>
        </TableContainer>
    )
}
