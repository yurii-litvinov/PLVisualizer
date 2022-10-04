import styled from 'styled-components'
import {FC} from "react";
import {Draggable} from "react-beautiful-dnd";

const Container = styled.div`
border: 1px solid lightblue;
padding: 8px;
margin-bottom: 8px;
border-radius: 2px`

interface lecturerProps {
    name: string
    index: number
}

export const Lecturer : FC<lecturerProps> =  ({name, index} : lecturerProps ) => {
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