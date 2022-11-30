import React, {Dispatch, FC, SetStateAction, useState} from "react";
import styled from "styled-components"
import {Droppable} from "react-beautiful-dnd";
import {DndDiscipline} from "./DndDiscipline";
import {Discipline} from "../../Models/Discipline";
import {Guid} from "guid-typescript";
import {Button} from "../Shared/Buttton";
import {DropDownItem} from "../Shared/DropDownItem";
import {DropDown} from "./DropDown";

export interface columnProps {
    handleResetClick : () => void
    setDisciplines: Dispatch<SetStateAction<Array<Discipline>>>
    disciplines: Array<Discipline>
}

interface disciplinesListProps {
    readonly isDraggingOver: boolean
}

export const DisciplinesColumn : FC<columnProps> = ({setDisciplines, disciplines, handleResetClick}) =>{
    const [dropDown, setDropDown] = useState(false)

    return ( <Container>
            <TitleContainer>
                <h3>Дисциплины</h3>
            </TitleContainer>
            <Button  color={'black'}
                     style={{width: "75%"}}
                     backgroundColor={'lightgrey'}
                     onHoverBackgroundColor={'lightblue'}
                     onClick={handleResetClick}> Сбросить </Button>
            <DropDownItem onClick={() => setDropDown(!dropDown)} >Сортировка</DropDownItem>
            {dropDown && <DropDown  setDisciplines={setDisciplines}
                                   setVisibility={setDropDown} />}
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
  overflow-x: hidden;
  height: 750px;
  display: flex;
  justify-content: center;
  align-items: center;
  align-content: center;
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
  width: 95%;
  min-height: 400px;
    padding: 8px;
  overflow-y: scroll;
  border: 1px solid lightblue;
  
  background-color: ${(props) => props.isDraggingOver ? 'skyblue' : 'white'}`

