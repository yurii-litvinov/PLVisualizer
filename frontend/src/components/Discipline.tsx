import styled from 'styled-components'
import {FC} from "react";
import {Draggable} from "react-beautiful-dnd";


interface disciplineProps {
    name: string
}

export const Discipline : FC<disciplineProps> =  ({name} : disciplineProps) => {
    return (
        <Draggable draggableId={name} index={0}>
            { (provided) => <Container
                {...provided.draggableProps}
                {...provided.dragHandleProps}
                ref={provided.innerRef}>
                {name}
            </Container>}
        </Draggable>
    )
}

const Container = styled.div`
border: 1px solid lightblue;
padding: 8px;
margin-bottom: 8px;
border-radius: 2px`