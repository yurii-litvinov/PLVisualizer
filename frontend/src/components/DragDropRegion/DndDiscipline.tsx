import styled from 'styled-components'
import {FC} from "react";
import {Draggable} from "react-beautiful-dnd";


interface disciplineProps {
    name: string
    index: number
}

interface containerProps  {
    readonly isDragging: boolean
}

export const DndDiscipline : FC<disciplineProps> =  ({name, index} : disciplineProps) => {
    return (
        <Draggable draggableId={name} index={index}>
            { (provided, snapshot) =>
                <Container
                {...provided.draggableProps}
                {...provided.dragHandleProps}
                ref={provided.innerRef}
                isDragging={snapshot.isDragging}>
                {name}
            </Container>}
        </Draggable>
    )
}

const Container = styled.div<containerProps>`
    border: 1px solid lightblue;
    padding: 8px;
    margin-bottom: 8px;
    border-radius: 2px;
    background-color: ${props => (props.isDragging ? 'lightgreen' : 'white')}
`