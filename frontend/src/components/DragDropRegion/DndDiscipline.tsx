import styled from 'styled-components'
import {FC} from "react";
import {Draggable} from "react-beautiful-dnd";
import {Discipline} from "../../Models/Discipline";


interface disciplineProps {
    discipline: Discipline
    index: number
}

interface containerProps  {
    readonly isDragging: boolean
}

export const DndDiscipline : FC<disciplineProps> =  ({discipline, index} : disciplineProps) => {
    return (
        <Draggable draggableId={discipline.id.toString()} index={index}>
            { (provided, snapshot) =>
                <Container
                {...provided.draggableProps}
                {...provided.dragHandleProps}
                ref={provided.innerRef}
                isDragging={snapshot.isDragging}>
                {discipline.content}
            </Container>}
        </Draggable>
    )
}

const Container = styled.div<containerProps>`
  font-size: 11px;
    border: 1px solid lightblue;
    padding: 8px;
    margin-bottom: 8px;
    border-radius: 2px;
    background-color: ${props => (props.isDragging ? 'lightgreen' : 'white')}
`