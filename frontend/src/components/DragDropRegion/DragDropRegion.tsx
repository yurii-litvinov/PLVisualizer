import {Dispatch, FC, SetStateAction} from "react";
import {DragDropContext, DropResult} from "react-beautiful-dnd";
import {DisciplinesColumn} from "./DisciplinesColumn";
import {LecturersTable} from "./LecturersTable";
import styled from "styled-components";
import { Lecturer } from "../../Models/Lecturer";
import {Discipline} from "../../Models/Discipline";

export interface dragDropRegionProps {
    lecturers : Array<Lecturer>
    setLecturers: Dispatch<SetStateAction<Array<Lecturer>>>
    columnDisciplines: Array<Discipline>
    setColumnDisciplines: Dispatch<SetStateAction<Array<Discipline>>>
}

export const DragDropRegion : FC<dragDropRegionProps>  = ({lecturers, setLecturers, columnDisciplines, setColumnDisciplines}) => {
    const handleResetClick = () => {
        setColumnDisciplines((disciplines) => {
            const newDisciplines = Array.from(disciplines)
            lecturers.forEach(lecturer => {
                lecturer.distributedLoad = 0
                lecturer.disciplines.forEach(discipline =>
                {
                    newDisciplines.splice(0,0,discipline)}
                )
            })
            setLecturers(lecturers => {
                const newLecturers = Array.from(lecturers)
                newLecturers.forEach(lecturer => lecturer.disciplines = [])
                return newLecturers
            })
            return newDisciplines
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
                const newLecturers = Array.from(lecturers)
                const lecturer = newLecturers.find(lecturer => lecturer.name === destination?.droppableId);
                const newLecturerDisciplines = Array.from(lecturer!.disciplines)
                newLecturerDisciplines.splice(destination!.index, 0, columnDisciplines[source.index])
                lecturer!.disciplines = newLecturerDisciplines
                lecturer!.distributedLoad += columnDisciplines[source.index].totalLoad
                return newLecturers
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
                const lecturer = lecturers.find(lecturer => lecturer.name === source.droppableId)
                const discipline = lecturer!.disciplines[source.index]
                disciplines.splice(destination.index, 0, discipline)
                return  disciplines
            })
            setLecturers((lecturers) => {
                const newLecturers = Array.from(lecturers)
                const lecturer = newLecturers.find(lecturer => lecturer.name === source.droppableId)
                const discipline = lecturer!.disciplines[source.index]
                lecturer!.distributedLoad -= discipline.totalLoad
                const newLecturerDisciplines = Array.from(lecturer!.disciplines)
                newLecturerDisciplines.splice(source.index, 1)
                lecturer!.disciplines = newLecturerDisciplines
                return newLecturers;
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
                const lecturer = lecturers.find(lecturer => lecturer.name === source.droppableId)
                lecturer!.disciplines.splice(destination!.index,0, lecturer!.disciplines[source.index])
                lecturer!.disciplines.splice(source.index+1,1)
                return lecturers
            })
        }  // another lecturer
        else if (result.source.droppableId !== destination.droppableId) {
            setLecturers(lecturers => {
                const newLecturers = Array.from(lecturers)
                const sourceLecturer = newLecturers.find(lecturer => lecturer.name === source.droppableId)
                const destinationLecturer = newLecturers.find(lecturer => lecturer.name === destination.droppableId)
                const discipline = sourceLecturer!.disciplines[source.index];
                destinationLecturer!.disciplines.splice(destination.index, 0, discipline )
                destinationLecturer!.distributedLoad += discipline.totalLoad
                sourceLecturer!.disciplines.splice(source.index, 1)
                sourceLecturer!.distributedLoad -= discipline.totalLoad
                return newLecturers
            })
            setColumnDisciplines(columnDisciplines => columnDisciplines)
        }
    }

    const handleDragEnd  = (result: DropResult) =>{
        const destination = result.destination
        const source = result.source
        if (!destination || (source.droppableId === destination.droppableId && source.index === destination.index)){
            return
        }
        // dnd affecting disciplines column
         if (destination?.droppableId === 'column' || source.droppableId === 'column'){
             handleDndAffectingColumn(result)}
        else {
            handleTableDnd(result)
        }
    }

    return(
    <DragDropContext onDragEnd={handleDragEnd}>
        <DragDropContextContainer>
            <DisciplinesColumn handleResetClick={handleResetClick} setDisciplines={setColumnDisciplines} disciplines={columnDisciplines}/>
            <LecturersTable  lecturers={lecturers}/>
        </DragDropContextContainer>
    </DragDropContext>)
}



const DragDropContextContainer = styled.div`
display: flex`
