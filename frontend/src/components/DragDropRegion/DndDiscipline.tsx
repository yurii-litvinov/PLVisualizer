import styled from 'styled-components'
import {FC} from "react";
import {Draggable} from "react-beautiful-dnd";


interface disciplineProps {
    content: string
    index: number
}

interface containerProps  {
    readonly isDragging: boolean
}

export const DndDiscipline : FC<disciplineProps> =  ({content, index} : disciplineProps) => {
    return (
        <Draggable draggableId={content} index={index}>
            { (provided, snapshot) =>
                <Container
                {...provided.draggableProps}
                {...provided.dragHandleProps}
                ref={provided.innerRef}
                isDragging={snapshot.isDragging}>
                {content}
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