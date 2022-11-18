import {Dispatch, FC, SetStateAction} from "react";
import {DragDropContext, DropResult} from "react-beautiful-dnd";
import {DisciplinesColumn} from "./DisciplinesColumn";
import {LecturersTable} from "./LecturersTable";
import styled from "styled-components";
import { Lecturer } from "../../Models/Lecturer";
import {Discipline} from "../../Models/Discipline";

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
            lecturers.map(lecturer => {
                lecturer.distributedLoad = 0
                lecturer.disciplines.map(discipline =>
                    newDisciplines.splice(0,0,discipline))
            })
            return newDisciplines
        })

        setLecturers((lecturers) => {
            const newLecturers = Array.from(lecturers)
            const newDisciplines = [] as Discipline[]
            newLecturers.map(lecturer => lecturer.disciplines = newDisciplines)
            return newLecturers
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
                lecturers[lecturerIndex].distributedLoad += columnDisciplines[source.index].contactLoad
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
                const lecturer = lecturers.find(lecturer => lecturer.name === source.droppableId)
                const discipline = lecturer!.disciplines[source.index]
                disciplines.splice(destination.index, 0, discipline)
                lecturer!.distributedLoad -= discipline.contactLoad
                return  disciplines
            })
            // тут тоже массив лекторов не копируется
            setLecturers((lecturers) => {
                const lecturer = lecturers.find(lecturer => lecturer.name === source.droppableId)
                lecturer!.disciplines.splice(source.index, 1)
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
                const lecturer = lecturers.find(lecturer => lecturer.name === source.droppableId)
                lecturer!.disciplines.splice(destination!.index,0, lecturer!.disciplines[source.index])
                lecturer!.disciplines.splice(source.index+1,1)
                return lecturers
            })
        }  // another lecturer
        else if (result.source.droppableId !== destination.droppableId) {
            setLecturers(lecturers => {
                const sourceLecturer = lecturers.find(lecturer => lecturer.name === source.droppableId)
                const destinationLecturer = lecturers.find(lecturer => lecturer.name === destination.droppableId)
                const discipline = sourceLecturer!.disciplines[source.index];
                destinationLecturer!.disciplines.splice(destination.index, 0, discipline )
                console.log('destination')
                console.log(destinationLecturer!.distributedLoad)
                destinationLecturer!.distributedLoad += discipline.contactLoad;
                console.log(destinationLecturer!.distributedLoad)
                sourceLecturer!.disciplines.splice(source.index, 1)
                console.log('source')
                console.log(sourceLecturer!.distributedLoad)
                sourceLecturer!.distributedLoad -= discipline.contactLoad
                console.log(sourceLecturer!.distributedLoad)
                return lecturers
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
            <DisciplinesColumn handleResetClick={handleResetClick} disciplines={columnDisciplines}/>
            <LecturersTable lecturers={lecturers}/>
        </DragDropContextContainer>
    </DragDropContext>)
}



const DragDropContextContainer = styled.div`
display: flex`
