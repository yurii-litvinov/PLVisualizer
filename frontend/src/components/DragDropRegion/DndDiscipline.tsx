import styled from 'styled-components'
import {FC} from "react";
import {Draggable} from "react-beautiful-dnd";
import {Discipline} from "../../Models/Discipline";
import { Card, CardContent, Typography } from '@mui/material';


interface DisciplineProps {
    discipline: Discipline
    index: number
}

interface ContainerProps  {
    readonly isDragging: boolean
}

export const DndDiscipline : FC<DisciplineProps> =  ({discipline, index} : DisciplineProps) => {
    return (
        <Draggable draggableId={discipline.id.toString()} index={index}>
            { (provided, snapshot) =>
                <Container
                    {...provided.draggableProps}
                    {...provided.dragHandleProps}
                    ref={provided.innerRef}
                    isDragging={snapshot.isDragging}>
                    <Card>
                        <CardContent>
                            <Typography sx={{ fontSize: 14 }} color="text.primary" gutterBottom>
                                [{discipline.code}] {discipline.name}
                            </Typography>
                            <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                                {discipline.workType.toLowerCase()}, {discipline.term.toLowerCase()}, {discipline.load} часов
                            </Typography>
                            <Typography sx={{ fontSize: 11 }} color="text.secondary" gutterBottom>
                                Контингент: {discipline.audience}
                            </Typography>
                        </CardContent>
                    </Card>
                </Container>}
        </Draggable>
    )
}

const Container = styled.div<ContainerProps>`
  font-size: 11px;
    border: 1px solid lightblue;
    padding: 8px;
    margin-bottom: 8px;
    border-radius: 2px;
    background-color: ${props => (props.isDragging ? 'lightgreen' : 'white')}
`