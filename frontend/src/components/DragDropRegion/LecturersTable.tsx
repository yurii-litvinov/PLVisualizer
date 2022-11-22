import React, {FC} from 'react'
import {Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@material-ui/core";
import {Droppable} from "react-beautiful-dnd";
import {Lecturer} from "../../Models/Lecturer";
import {DndDiscipline} from "./DndDiscipline";
import {Guid} from "guid-typescript";

export interface tableProps {
    lecturers: Lecturer[]
}


/// Represents a table with a pedagogical load with the possibility of Drag&Drop
export const LecturersTable : FC<tableProps> = (tableData) => {
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
                    {tableData.lecturers.map((lecturer) => {
                        return <TableRow>
                            <TableCell align={"left"}>{lecturer.name}</TableCell>
                            <TableCell align={"left"}>{lecturer.post}</TableCell>
                            <TableCell align={"left"}>{lecturer.interestRate}</TableCell>
                            <Droppable droppableId={lecturer.name}>
                                {(provided, snapshot) => { return(
                                    <TableCell
                                        style={{backgroundColor: snapshot.isDraggingOver ? 'skyblue' : 'white'}}
                                        ref={provided.innerRef}
                                    >
                                            {lecturer.disciplines.map((discipline, index) =>
                                            {
                                                const id = discipline.id.toString()
                                                console.log(id)
                                                return(
                                                <DndDiscipline  index={index} key={discipline.id.toString()} discipline={discipline} />
                                            )
                                            })}
                                        {provided.placeholder}
                                    </TableCell>)
                                }
                                }
                            </Droppable>
                            <TableCell align={"left"}>{lecturer.distributedLoad}</TableCell>
                            <TableCell align={"left"}>{lecturer.standard}</TableCell>
                        </TableRow>
                    })}
                    </TableBody>
            </Table>
        </TableContainer>
    )
}
