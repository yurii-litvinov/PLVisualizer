import React, {FC} from "react";
import styled from "styled-components"
import {Droppable} from "react-beautiful-dnd";
import {Discipline} from "./Discipline";

export interface columnProps {
    disciplineIds: string[]
}

interface disciplinesListProps {
    readonly isDraggingOver: boolean
}

export const DisciplinesColumn : FC<columnProps> = (columnsProps) =>{
    return ( <Container>
            <Title>Дисциплины</Title>
            <Droppable droppableId={'column'}>
                {(provided, snapshot) =>(
                    <DisciplinesList ref={provided.innerRef} isDraggingOver = {snapshot.isDraggingOver}>
                        {columnsProps.disciplineIds.map((id, index) => {
                            return(<Discipline key={id} name={id} index={index}/>)
                        })}
                        {provided.placeholder}
                    </DisciplinesList>)}
            </Droppable>
        </Container>
    )
}

const Container = styled.div`
  flex-direction: column;
    width: 15%;
    margin: 8px;
    border: 1px solid lightblue; 
    border-radius: 2px`

const Title = styled.h3`
    padding: 8px;`

const DisciplinesList = styled.div<disciplinesListProps>`
  min-height: 200px;
    padding: 8px;
  background-color: ${(props) => props.isDraggingOver ? 'skyblue' : 'white'}`