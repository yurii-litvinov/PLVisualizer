import React, {FC, useState} from 'react'
import {Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@material-ui/core";
import {DragDropContext, Droppable, DropResult} from "react-beautiful-dnd";
import {tableRow} from "./tableRow";
import {Discipline} from "./Discipline";
import styled from 'styled-components'

interface tableProps {
    lecturersIds : string[]
    lecturers : {[key:string] : tableRow}
    disciplines : {[key:string] : string}
    
}

export const DisciplinesTable : FC<tableProps> = ({lecturers, lecturersIds, disciplines} : tableProps) => {
    const [tableData, setTableData] = useState<tableProps>({lecturers, lecturersIds, disciplines})


    const onDragEnd = (result: DropResult) => {
        if ((!result.destination) || (result.destination.index === result.source.index &&
            result.destination.droppableId === result.source.droppableId)){
            return
        }

        if (result.destination.droppableId === result.source.droppableId){
            setTableData((previousState) => {
                const {lecturers} = previousState
                const lecturer = lecturers[result.source.droppableId]
                const newDisciplineIds = Array.from(lecturer.disciplineIds)
                newDisciplineIds.splice(result.source.index,1)
                newDisciplineIds.splice(result.destination!.index,0, result.draggableId)

                const newLecturer = {
                    ...lecturer,
                    disciplineIds : newDisciplineIds
                }

                return {
                    ...previousState,
                    lecturers: {
                        ...lecturers,
                        [result.source.droppableId] : newLecturer
                    }
                }
            })
        }

        if (result.source.droppableId !== result.destination.droppableId) {
            setTableData(({lecturers, lecturersIds, disciplines}) => {
                const destination = result.destination
                const source = result.source

                const sourceIds = Array.from(lecturers[source.droppableId].disciplineIds)
                sourceIds.splice(source.index, 1)
                const destinationIds = Array.from(lecturers[destination!.droppableId].disciplineIds)
                destinationIds.splice(destination!.index, 0, result.draggableId)
                lecturers[destination!.droppableId].disciplineIds = destinationIds;
                lecturers[source!.droppableId].disciplineIds = sourceIds
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
                    {tableData.lecturersIds.map((lecturerId) => {
                        return <TableRow>
                            <TableCell align={"left"}>{tableData.lecturers[lecturerId].name}</TableCell>
                            <TableCell align={"left"}>{tableData.lecturers[lecturerId].post}</TableCell>
                            <TableCell align={"left"}>{tableData.lecturers[lecturerId].interestRate}%</TableCell>
                            <Droppable droppableId={tableData.lecturers[lecturerId].name}>
                                {(provided) => { return(
                                    <TableCell ref={provided.innerRef} >
                                            {tableData.lecturers[lecturerId].disciplineIds.map((disciplineId, index) =>
                                            { return(
                                                <Discipline name={disciplineId} index={index} />
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

export const DroppableContainer = styled.div`
display: flex;
flex-direction: column`


