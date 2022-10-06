import React, {FC, useState} from 'react'
import {Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@material-ui/core";
import {DragDropContext, Draggable, Droppable, DropResult} from "react-beautiful-dnd";
import {tableRow} from "./TableRow";
import {Discipline} from "./Discipline";

export const DisciplinesTable : FC<{items : tableRow[]}> = ({items}) => {
    const [localItems, setLocalItems] = useState<Array<tableRow>>(items)


    const onDragEnd = (result: DropResult) => {
        if (!result.destination || result.destination.index === result.source.index){
            return
        }

        setLocalItems(previous  => {
            const temp = [...previous]
            const destination = temp[result.destination!.index]
            const source = temp[result.source!.index]
            //const destination1 =
            // temp[result.destination!.index] = temp[result.source.index]
            // temp[result.source.index] = d
            // return temp;
            return temp
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
                    {localItems.map((item) => {
                        return <TableRow>
                            <TableCell align={"left"}>{item.name}</TableCell>
                            <TableCell align={"left"}>{item.post}</TableCell>
                            <TableCell align={"left"}>{item.interestRate}%</TableCell>
                            <Droppable droppableId={item.name}>
                                {(provided) => { return(
                                    <TableCell ref={provided.innerRef}>
                                            {item.disciplines.map(discipline =>
                                            { return(
                                                <Discipline name={discipline.name} index={discipline.index}/>
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

