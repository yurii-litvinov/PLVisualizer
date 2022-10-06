import React, {FC, useState} from 'react'
import {Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@material-ui/core";
import {DragDropContext, Draggable, Droppable, DropResult} from "react-beautiful-dnd";
import {tableRow} from "./TableRow";
import {Discipline} from "./Discipline";

interface tableProps {
    lecturersIds : string[]
    lecturers : {[key:string] : tableRow}
    
}

export const DisciplinesTable : FC<tableProps> = ({lecturers, lecturersIds} : tableProps) => {
    const [tableData, setTableData] = useState<tableProps>({lecturers, lecturersIds})


    const onDragEnd = (result: DropResult) => {
        if (!result.destination || result.destination.index === result.source.index){
            return
        }

        setTableData((previous)  => {
            const tempPrevious = previous;
            const destination = result.destination
            const source = result.source

            tempPrevious.lecturers[source.droppableId].disciplinesIds.splice(source.index, 1)

            return tempPrevious
        })

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
                    {tableData.lecturersIds.map((id) => {
                        return <TableRow>
                            <TableCell align={"left"}>{lecturers[id].name}</TableCell>
                            <TableCell align={"left"}>{lecturers[id].post}</TableCell>
                            <TableCell align={"left"}>{lecturers[id].interestRate}%</TableCell>
                            <Droppable droppableId={lecturers[id].name}>
                                {(provided) => { return(
                                    <TableCell ref={provided.innerRef}>
                                            {lecturers[id].disciplinesIds.map((disId) =>
                                            { return(
                                                <Discipline name={lecturers[id].disciplines[disId]} />
                                            )
                                            })}
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

