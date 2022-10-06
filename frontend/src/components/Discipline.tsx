import styled from 'styled-components'
import {FC} from "react";
import {Draggable} from "react-beautiful-dnd";

export interface Discipline{
    index: number
    name: string
}

export interface disciplineProps{
    index: number
    name: string
}

const Container = styled.div`
border: 1px solid lightblue;
padding: 8px;
margin-bottom: 8px;
border-radius: 2px`


export const Discipline : FC<Discipline> =  ({name, index} : disciplineProps ) => {
    return (
        <Draggable draggableId={name} index={index}>
            { (provided) => <Container
                {...provided.draggableProps}
                {...provided.dragHandleProps}
                ref={provided.innerRef}>
                {name}
            </Container>}
        </Draggable>
    )
}