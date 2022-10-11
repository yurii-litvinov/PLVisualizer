import React, {FC} from "react";
import styled from "styled-components"
import {Droppable} from "react-beautiful-dnd";
import {Discipline} from "./Discipline";
import {ITableRow} from "./ITableRow";

interface columnProps {
    disciplineIds: string[]
    lecturers : {[key:string] : ITableRow}
    disciplines : {[key:string] : string}
}

export const DisciplinesColumn : FC<columnProps> = ({disciplineIds, lecturers, disciplines} : columnProps) =>{
    return ( <Container>
            <Title>Дисциплины</Title>
            <Droppable droppableId={'Дисциплины'}>
                {(provided) =>(
                    <Children ref={provided.innerRef}>
                        {disciplineIds.map((id, index) => {
                            return(<Discipline key={id} name={id} index={index}/>)
                        })}
                        {provided.placeholder}
                    </Children>)}
            </Droppable>
        </Container>
    )
}

const Container = styled.div`
    width: 15%;
    margin: 8px;
    border: 1px solid lightblue; 
    border-radius: 2px`

const Title = styled.h3`
    padding: 8px;`

const Children = styled.div`
    padding: 8px`