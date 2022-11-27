import React, {FC} from "react";
import styled from "styled-components"
import {Droppable} from "react-beautiful-dnd";
import {DndDiscipline} from "./DndDiscipline";
import {Discipline} from "../../Models/Discipline";
import {Guid} from "guid-typescript";
import {Button} from "../Shared/Buttton";

export interface columnProps {
    handleResetClick : () => void
    disciplines: Discipline[]
}

interface disciplinesListProps {
    readonly isDraggingOver: boolean
}

export const DisciplinesColumn : FC<columnProps> = ({disciplines, handleResetClick}) =>{
    return ( <Container>
            <TitleContainer>
                <h3>Дисциплины</h3>
            </TitleContainer>
            <Button color={'black'} backgroundColor={'lightgrey'} onHoverBackgroundColor={'lightblue'} onClick={handleResetClick}> Сбросить </Button>
            <Droppable droppableId={'column'}>
                {(provided, snapshot) =>(
                    <DisciplinesList ref={provided.innerRef} isDraggingOver = {snapshot.isDraggingOver}>
                        {disciplines.map((discipline, index) => {
                            return(<DndDiscipline key={discipline.id.toString()} discipline={discipline} index={index}/>)
                        })}
                        {provided.placeholder}
                    </DisciplinesList>)}
            </Droppable>
        </Container>
    )
}

const Container = styled.div`
  height: 100%;
  display: flex;
  flex-direction: column;
    width: 25%;
    margin: 8px;
    border: 1px solid lightblue; 
    border-radius: 2px`

const TitleContainer = styled.div`
  align-items: center;
  align-content: center;
  justify-content: center;
  justify-items: center`


const DisciplinesList = styled.div<disciplinesListProps>`
  min-height: 400px;
    padding: 8px;
  overflow-y: scroll;
  background-color: ${(props) => props.isDraggingOver ? 'skyblue' : 'white'}`

