import React, {FC, useState} from 'react'
import {Lecturer} from "../../Models/Lecturer";
import styled from "styled-components";
import {Droppable} from "react-beautiful-dnd";
import {DndDiscipline} from "./DndDiscipline";
import IconButton, { IconButtonProps } from '@mui/material/IconButton';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';


interface ExpandMoreProps extends IconButtonProps {
    expand: boolean;
}

const ExpandMore = styled((props: ExpandMoreProps) => {
    const { expand, ...other } = props;
    return <IconButton {...other} />;
})(({ theme, expand }) => ({
    transform: !expand ? 'rotate(0deg)' : 'rotate(180deg)',
    marginLeft: 'auto',
}));

export interface LecturerRowProps {
    lecturer: Lecturer
}

const LecturerRow : FC<LecturerRowProps> = ({lecturer}) => {
    const [expanded, setExpanded] = useState(true);

    const handleExpandClick = () => {
        setExpanded(!expanded)
    };

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

    const loadType = getLoadType(lecturer.distributedLoad, lecturer.requiredLoad)

    return(<Droppable droppableId={lecturer.name}>
                {((provided, snapshot) => {
                    return (
                        <TableRow ref={provided.innerRef} style={{backgroundColor : snapshot.isDraggingOver ? 'skyblue' : 'white'}}>
                            <div>
                                <ExpandMore 
                                    expand={expanded}
                                    onClick={handleExpandClick}
                                    >
                                    <ExpandMoreIcon />
                                </ExpandMore>
                            </div>
                            <NameCell>{lecturer.name}</NameCell>
                            <PositionCell>{lecturer.position}</PositionCell>
                            <InterestRateCell>{lecturer.fullTimePercent}</InterestRateCell>
                            <DisciplinesCell>{expanded ? lecturer.disciplines.map((discipline, index) => {
                                    return (<DndDiscipline discipline={discipline} index={index} key={discipline.id.toString()}/>)
                                }) : []}
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
    )

}


export interface TableProps {
    lecturers: Array<Lecturer>
}

/// Represents a table with a pedagogical load with the possibility of Drag&Drop
export const LecturersTable : FC<TableProps> = ({lecturers}) => {

    return(
        <TableContainer>
            <TableRow>
                <TableHeader style={{width: "1.5%"}}></TableHeader>
                <TableHeader style={{width: "13%"}}>ФИО</TableHeader>
                <TableHeader style={{width: "10%"}}>Должность</TableHeader>
                <TableHeader style={{width: "10%"}}>Процент ставки</TableHeader>
                <TableHeader style={{width: "40%"}}>Дисциплины</TableHeader>
                <TableHeader style={{width: "15%"}}>Распределенная нагрузка</TableHeader>
                <TableHeader style={{width: "5%"}}>Ожидаемая нагрузка</TableHeader>
            </TableRow>
            {lecturers.map(lecturer => <LecturerRow lecturer={lecturer} />)}

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
  width: 13%`

const PositionCell = styled.div`
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
            return 'Red'
        case "Ниже нормы":
            return 'LightCoral'
        case "Несколько ниже нормы":
            return 'NavajoWhite'
        case "Нормальная нагрузка":
            return 'LightGreen'
        case "Несколько выше нормы":
            return  'MediumAquaMarine'
        case "Выше нормы":
            return "MediumTurquoise"
        case "Сильно выше нормы":
            return "RoyalBlue"
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
  margin-top: 3%;
  margin-left: 15.5%;
  width: 100%;
  display: flex;
  flex-direction: column`


