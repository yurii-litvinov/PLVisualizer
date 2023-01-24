import React, {Dispatch, FC, SetStateAction} from "react";
import styled from "styled-components"
import {Droppable} from "react-beautiful-dnd";
import {DndDiscipline} from "./DndDiscipline";
import {Discipline} from "../../Models/Discipline";

export interface columnProps {
    handleResetClick : () => void
    setDisciplines: Dispatch<SetStateAction<Array<Discipline>>>
    disciplines: Array<Discipline>
}

interface disciplinesListProps {
    readonly isDraggingOver: boolean
}

export const DisciplinesColumn : FC<columnProps> = ({setDisciplines, disciplines, handleResetClick}) =>{
    return ( <Container>
            <TitleContainer>
                <h3>Место для нераспределённых курсов</h3>
            </TitleContainer>
                {/* <Button  color={'black'}
                         style={{width: "75%", border:"5 px solid black"}}
                         backgroundColor={'white'}
                         onHoverBackgroundColor={'lightblue'}
                         onClick={handleResetClick}> Сбросить </Button> */}
                 {/* <DropDown  setDisciplines={setDisciplines}/> */}
            <Droppable droppableId={'column'}>
                {(provided, snapshot) =>(
                    <DisciplinesList placeholder={"Дисциплины можно перетаскивать сюда"} ref={provided.innerRef} isDraggingOver={snapshot.isDraggingOver}>
                        {disciplines.map((discipline, index) => {
                            return(<DndDiscipline key={discipline.id.toString()} discipline={discipline} index={index}/>)
                        })}
                        {provided.placeholder}
                    </DisciplinesList>
                )}
            </Droppable>
        </Container>
    )
}

const Container = styled.div`
  margin-top: 3%;
  position: fixed;
  overflow-x: hidden;
  height: 100%;
  display: flex;
  align-items: center;
  align-content: center;
  flex-direction: column;
  width: 15%;
  border: 1px solid lightblue; 
  border-radius: 2px`

const TitleContainer = styled.div`
  display: flex;
  align-items: center;
  align-content: center;
  justify-content: center;
  justify-items: center`


const DisciplinesList = styled.div<disciplinesListProps>`
  width: 95%;
  height: 100%;
  min-height: 100px;
  padding: 8px;
  overflow-y: visible;
  background-color: ${(props) => props.isDraggingOver ? 'skyblue' : 'white'}`

