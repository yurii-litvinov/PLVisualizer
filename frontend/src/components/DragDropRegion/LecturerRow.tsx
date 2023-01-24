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

export const LecturerRow : FC<LecturerRowProps> = ({lecturer}) => {
    const [expanded, setExpanded] = useState(false);

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
                            <NameCell isFired={lecturer.isFired}>{lecturer.name}</NameCell>
                            <PositionCell>{lecturer.position}</PositionCell>
                            <FullTimePercentCell>{lecturer.fullTimePercent}</FullTimePercentCell>
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

interface NameCellProps {
    isFired: boolean
}

const NameCell = styled.div<NameCellProps>`
  margin-left: 8px;
  width: 13%;
  color: ${(props) => props.isFired ? "Red" : "Black"}
`

const PositionCell = styled.div`
  margin-left: 8px;
  width: 10%`

const FullTimePercentCell = styled.div`
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
  background-color: ${(props) =>  getCellColor(props.loadType)};
  padding: 5px
`

const StandardCell = styled.div`
  margin-left: 8px;
  width: 5%; `

const TableRow = styled.div`
  display: flex;
  box-sizing: border-box;
  flex-direction: row;
  flex-wrap: wrap;
  border: 1px solid lightblue;
`