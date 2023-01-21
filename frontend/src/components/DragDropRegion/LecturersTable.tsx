import React, {FC} from 'react'
import {Lecturer} from "../../Models/Lecturer";
import styled from "styled-components";
import {Droppable} from "react-beautiful-dnd";
import {DndDiscipline} from "./DndDiscipline";

export interface tableProps {
    lecturers: Array<Lecturer>
}


/// Represents a table with a pedagogical load with the possibility of Drag&Drop
export const LecturersTable : FC<tableProps> = ({lecturers}) => {
    const getLoadType = (distributedLoad : number, standard: number) : string => {
        if (distributedLoad === 0) return 'Сильно ниже нормы'
        const frac = distributedLoad / standard
        if (frac < 0.5) return 'Сильно ниже нормы'
        else if (frac < 0.8) return 'Ниже нормы'
        else if (frac < 1) return 'Несколько ниже нормы'
        else if (frac < 1.2) return 'Нормальная нагрузка'
        else if (frac < 1.5) return 'Несколько выше нормы'
        else if (frac < 2 ) return 'Выше нормы'
        return 'Сильно выше нормы'
    }

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
                const loadType = getLoadType(lecturer.distributedLoad, lecturer.requiredLoad)
                return <Droppable droppableId={lecturer.name}>
                    {((provided, snapshot) => {
                        return (
                            <TableRow ref={provided.innerRef} style={{backgroundColor : snapshot.isDraggingOver ? 'skyblue' : 'white'}}>
                                <NameCell>{lecturer.name}</NameCell>
                                <PostCell>{lecturer.position}</PostCell>
                                <InterestRateCell>{lecturer.fullTimePercent}</InterestRateCell>
                                <DisciplinesCell>{lecturer.disciplines.map((discipline, index) => {
                                    return (<DndDiscipline discipline={discipline} index={index} key={discipline.id.toString()}/>)
                                })}
                                </DisciplinesCell>
                                <DistributedLoadCell
                                    loadType={loadType}
                                >
                                    <div>{lecturer.distributedLoad}</div>
                                    <div>{loadType}</div>
                                </DistributedLoadCell>
                                <StandardCell>{lecturer.requiredLoad}</StandardCell>
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
  margin-left: 8px;
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
  margin-left: 8px;
  width: 10%`
const PostCell = styled.div`
  margin-left: 8px;
  width: 10%`
const InterestRateCell = styled.div`
  margin-left: 8px;
  width: 10%`
const DisciplinesCell = styled.div`
  margin-left: 8px;
  width: 40%`

interface distributedLoadCellProps {
    loadType: string
}

const getCellColor = (loadType: string) : string =>{
    switch (loadType){
        case "Сильно ниже нормы":
            return 'white'
        case "Ниже нормы":
            return 'lightblue'
        case "Несколько ниже нормы":
            return 'lightgreen'
        case "Нормальная нагрузка":
            return 'yellow'
        case "Несколько выше нормы":
            return  'pink'
        case "Выше нормы":
            return "deeppink"
        case "Сильно выше нормы":
            return "crimson"
    }
    return 'white'
}
const DistributedLoadCell = styled.div<distributedLoadCellProps>`
  margin-left: 8px;
  width: 15%;
  background-color: ${(props) =>  getCellColor(props.loadType)}`
const StandardCell = styled.div`
  margin-left: 8px;
  width: 5%; `

const TableContainer = styled.div`
  margin-top: 6%;
  margin-left: 26%;
  width: 100%;
  display: flex;
  flex-direction: column`


