import React, {FC, useState} from "react";
import styled from "styled-components"
import {Droppable} from "react-beautiful-dnd";
import {DndDiscipline} from "./DndDiscipline";
import {Discipline} from "../../Models/Discipline";

export interface columnProps {
    handleResetClick : () => void
    disciplines: Discipline[]
}

interface disciplinesListProps {
    readonly isDraggingOver: boolean
}

export const DisciplinesColumn : FC<columnProps> = ({disciplines, handleResetClick}) =>{
    return ( <Container>
            <Title>Дисциплины</Title>
            <Droppable droppableId={'column'}>
                {(provided, snapshot) =>(
                    <DisciplinesList ref={provided.innerRef} isDraggingOver = {snapshot.isDraggingOver}>
                        {disciplines.map((discipline, index) => {
                            return(<DndDiscipline key={`${discipline.content} $name={id}`} content={discipline.content} index={index}/>)
                        })}
                        {provided.placeholder}
                    </DisciplinesList>)}
            </Droppable>
            <ButtonContainer onClick={handleResetClick}> Сбросить </ButtonContainer>
        </Container>
    )
}

const Container = styled.div`
  height: 600px;
  display: flex;
  flex-direction: column;
    width: 15%;
    margin: 8px;
    border: 1px solid lightblue; 
    border-radius: 2px`

const Title = styled.h3`
    padding: 8px;`

const DisciplinesList = styled.div<disciplinesListProps>`
  min-height: 50px;
    padding: 8px;
  background-color: ${(props) => props.isDraggingOver ? 'skyblue' : 'white'}`

const ButtonContainer = styled.button`
margin: 10px;
    appearance: none;
    backface-visibility: hidden;
    background-color: lightblue;
    border-radius: 9999px;
    border: 3px solid lightblue;
    box-sizing: border-box;
    color: black;
    cursor: pointer;
    font-family: "Segoe UI";
    font-size: 16px;
    font-weight: 600;
    line-height: 1.5;
    overflow: visible ;
    padding: 13px 20px;
`