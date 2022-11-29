import React, {FC} from 'react'
import {Lecturer} from "../../Models/Lecturer";
import styled from "styled-components";
import {Droppable} from "react-beautiful-dnd";
import {DndDiscipline} from "./DndDiscipline";

export interface tableProps {
    lecturers: Lecturer[]
}


/// Represents a table with a pedagogical load with the possibility of Drag&Drop
export const LecturersTable : FC<tableProps> = ({lecturers}) => {
    return(
        <TableContainer>
            <TableRow>
                <TableHeader style={{width: "10%"}}>ФИО</TableHeader>
                <TableHeader style={{width: "10%"}}>Должность</TableHeader>
                <TableHeader style={{width: "10%"}}>Процент ставки</TableHeader>
                <TableHeader style={{width: "40%"}}>Дисциплины</TableHeader>
                <TableHeader style={{width: "15%"}}>Распределенная нагрузка</TableHeader>
                <TableHeader style={{width: "5%"}}>Стандарт</TableHeader>
            </TableRow>
            {lecturers.map(lecturer => {
                return <Droppable droppableId={lecturer.name}>
                    {((provided, snapshot) => {
                        return (
                            <TableRow ref={provided.innerRef} style={{backgroundColor : snapshot.isDraggingOver ? 'skyblue' : 'white'}}>
                                <NameCell>{lecturer.name}</NameCell>
                                <PostCell>{lecturer.post}</PostCell>
                                <InterestRateCell>{lecturer.interestRate}</InterestRateCell>
                                <DisciplinesCell>{lecturer.disciplines.map((discipline, index) => {
                                    return (<DndDiscipline discipline={discipline} index={index} key={discipline.id.toString()}/>)
                                })}
                                </DisciplinesCell>
                                <DistributedLoadCell>{lecturer.distributedLoad}</DistributedLoadCell>
                                <StandardCell>{lecturer.standard}</StandardCell>
                                {provided.placeholder}
                            </TableRow>
                        )
                    })}
                </Droppable>
            })}

        </TableContainer>
    )
}

const TableHeader = styled.div`
    margin-left: 16px;
    font-weight: 600;
    display: flex;
    flex-flow: row wrap;
    transition: 0.5s`

const TableRow = styled.div`
    display: flex;
    box-sizing: border-box;
    flex-direction: row;
    flex-wrap: wrap;
    border: 1px solid lightblue;
`

const NameCell = styled.div`
  margin-left: 16px;
  width: 10%`
const PostCell = styled.div`
  margin-left: 16px;
  width: 10%`
const InterestRateCell = styled.div`
  margin-left: 16px;
  width: 10%`
const DisciplinesCell = styled.div`
  margin-left: 16px;
  width: 40%`
const DistributedLoadCell = styled.div`
  margin-left: 16px;
  width: 15%;
  background-color: pink`
const StandardCell = styled.div`
  margin-left: 16px;
  width: 5%`

const TableContainer = styled.div`
    width: 100%;
    display: flex;
    flex-direction: column`



