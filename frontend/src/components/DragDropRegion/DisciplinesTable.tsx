import React, {FC, useState} from 'react'
import {Table, TableBody, TableCell, TableCellProps, TableContainer, TableHead, TableRow} from "@material-ui/core";
import {DragDropContext, Droppable, DroppableProvided, DropResult} from "react-beautiful-dnd";
import {ITableRow} from "./ITableRow";
import {Discipline} from "./Discipline";

export interface tableProps {
    lecturerIds : string[]
    lecturers : {[key:string] : ITableRow}
    disciplines : {[key:string] : string}
}


/// Represents a table with a pedagogical load with the possibility of Drag&Drop
export const DisciplinesTable : FC<tableProps> = (tableData) => {
    return(
        <TableContainer>
            <Table style={{width: 1300}}>
                <colgroup>
                    <col style={{width: "10%"}}/>
                    <col style={{width: "10%"}}/>
                    <col style={{width: "3%"}}/>
                    <col style={{width: "20%"}}/>
                    <col style={{width: "15%"}}/>
                    <col style={{width: "7%"}}/>
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
                    <TableBody>
                    {tableData.lecturerIds.map((lecturerId) => {
                        return <TableRow>
                            <TableCell align={"left"}>{tableData.lecturers[lecturerId].name}</TableCell>
                            <TableCell align={"left"}>{tableData.lecturers[lecturerId].post}</TableCell>
                            <TableCell align={"left"}>{tableData.lecturers[lecturerId].interestRate}%</TableCell>
                            <Droppable droppableId={tableData.lecturers[lecturerId].name}>
                                {(provided, snapshot) => { return(
                                    <TableCell
                                        style={{backgroundColor: snapshot.isDraggingOver ? 'skyblue' : 'white'}}
                                        ref={provided.innerRef}
                                    >
                                            {tableData.lecturers[lecturerId].disciplineIds.map((disciplineId, index) =>
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
            </Table>
        </TableContainer>
    )
}
