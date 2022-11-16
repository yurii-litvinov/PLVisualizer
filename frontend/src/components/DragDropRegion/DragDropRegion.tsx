import {Dispatch, FC, SetStateAction, useState} from "react";
import {DragDropContext, DropResult} from "react-beautiful-dnd";
import {columnProps, DisciplinesColumn} from "./DisciplinesColumn";
import {LecturersTable, tableProps} from "./LecturersTable";
import styled from "styled-components";
import { Lecturer } from "../../Models/Lecturer";
import {Discipline} from "../../Models/Discipline";
import {duration} from "@material-ui/core";


export interface dragDropRegionProps {
    lecturers : Lecturer[]
    setLecturers: Dispatch<SetStateAction<Lecturer[]>>
    columnDisciplines: Discipline[]
    setColumnDisciplines: Dispatch<SetStateAction<Discipline[]>>
}

export const DragDropRegion : FC<dragDropRegionProps>  = ({lecturers, setLecturers, columnDisciplines, setColumnDisciplines}) => {
    const handleResetClick = () => {
        setColumnDisciplines((disciplines) => {
            const newDisciplines = Array.from(disciplines)
            lecturers.map(lecturer => lecturer.disciplines.map(discipline =>
                    newDisciplines.splice(0,0,discipline)))
            disciplines = newDisciplines;
            return disciplines
        })

        setLecturers((lecturers) => {
            const newLecturers = Array.from(lecturers)
            const newDisciplines = [] as Discipline[]
            newLecturers.map(lecturer => lecturer.disciplines = newDisciplines)
            lecturers = newLecturers
            return lecturers
        })
    }

    const handleDndAffectingColumn = (result: DropResult) => {
        const destination = result.destination
        const source = result.source
        //dnd between disciplines in column
        if (destination!.droppableId === source.droppableId){
            setColumnDisciplines((disciplines) => {
                const newDisciplines = Array.from(disciplines)
                newDisciplines.splice(destination!.index, 0, disciplines[source.index])
                newDisciplines.splice(source.index, 1)
                disciplines = newDisciplines
                return disciplines
            })
        }
        //dnd from column to table
            // тут надо посмотреть, нормально ли работает без копирования всего массива лекторов
        else if (source.droppableId === 'column'){
            setLecturers((lecturers) => {
                const lecturerIndex = lecturers.findIndex(lecturer => lecturer.name === destination?.droppableId);
                const newLecturerDisciplines = Array.from(lecturers[lecturerIndex].disciplines)
                newLecturerDisciplines.splice(destination!.index, 0, columnDisciplines[source.index])
                lecturers[lecturerIndex].disciplines = newLecturerDisciplines
                return lecturers
            })

            setColumnDisciplines((disciplines) => {
                const newDisciplines = Array.from(disciplines)
                newDisciplines.splice(source.index, 1)
                disciplines = newDisciplines
                return disciplines
            })
        }
        //dnd from table to column
        else if (destination?.droppableId === 'column'){
            setColumnDisciplines((disciplines) => {
                const lecturerIndex = lecturers.findIndex(lecturer => lecturer.name === source.droppableId)
                const newDisciplines = Array.from(disciplines)
                newDisciplines.splice(destination.index, 0, lecturers[lecturerIndex].disciplines[source.index])
                disciplines = newDisciplines;
                return disciplines
            })
            // тут тоже массив лекторов не копируется
            setLecturers((lecturers) => {
                const lecturerIndex = lecturers.findIndex(lecturer => lecturer.name == source.droppableId)
                const newDisciplines = Array.from(lecturers[lecturerIndex].disciplines)
                newDisciplines.splice(source.index, 1)
                lecturers[lecturerIndex].disciplines  = newDisciplines;
                return lecturers;
            })
        }
    }

    const handleTableDnd = (result: DropResult) => {
        const destination = result.destination
        const source = result.source

        // dnd to nowhere or to the same place
        if ((!destination) || (destination.index === source.index &&
            destination.droppableId === source.droppableId)){
            return
        }  // the same lecturer
        else if (destination!.droppableId === source.droppableId){
            setLecturers(lecturers => {
                const lecturerIndex = lecturers.findIndex(lecturer => lecturer.name === source.droppableId)
                const newLecturers = Array.from(lecturers)
                newLecturers[lecturerIndex].disciplines.splice(destination!.index,0, lecturers[lecturerIndex].disciplines[source.index])
                newLecturers[lecturerIndex].disciplines.splice(source.index,1)
                lecturers = newLecturers
                return lecturers
            })
        }  // another lecturer
        else if (result.source.droppableId !== destination.droppableId) {
            setLecturers(lecturers => {
                const newLecturers = Array.from(lecturers)
                const sourceLecturerIndex = lecturers.findIndex(lecturer => lecturer.name === source.droppableId)
                const destinationLecturerIndex = lecturers.findIndex(lecturer => lecturer.name === destination.droppableId)
                newLecturers[destinationLecturerIndex].disciplines.splice(destination.index, 0, lecturers[sourceLecturerIndex].disciplines[source.index] )
                newLecturers[sourceLecturerIndex].disciplines.splice(source.index, 1)
                lecturers = newLecturers
                return lecturers
            })
        }
    }

    const handleDragEnd  = (result: DropResult) =>{
        const destination = result.destination
        const source = result.source
        // dnd affecting disciplines column
        if (!destination || (source.droppableId === destination.droppableId && source.index === destination.index)){
            return
        }
        // if (destination?.droppableId === 'column' || source.droppableId === 'column'){
        //     handleDndAffectingColumn(result)
        // }
        else {
            handleTableDnd(result)
        }
    }

    return(
    <DragDropContext onDragEnd={handleDragEnd}>
        <DragDropContextContainer>
            <DisciplinesColumn handleResetClick={handleResetClick} disciplines={columnDisciplines}/>
            <LecturersTable lecturers={lecturers}/>
        </DragDropContextContainer>
    </DragDropContext>)
}



const DragDropContextContainer = styled.div`
display: flex`
