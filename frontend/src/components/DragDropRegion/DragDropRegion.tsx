import {Dispatch, FC, SetStateAction, useState} from "react";
import {DragDropContext, DropResult} from "react-beautiful-dnd";
import {columnProps, DisciplinesColumn} from "./DisciplinesColumn";
import {LecturersTable, tableProps} from "./LecturersTable";
import styled from "styled-components";
import { Lecturer } from "../../Models/Lecturer";
import {Discipline} from "../../Models/Discipline";


export interface dragDropRegionProps {
    lecturers : Lecturer[]
    disciplines: {[key:string]:Discipline}
    setLecturers: Dispatch<SetStateAction<Lecturer[]>>
}

export const DragDropRegion : FC<dragDropRegionProps>  = ({lecturers, disciplines, setLecturers}) => {
    // const handleResetClick = () => {
        // setColumnData(({disciplineIds, handleResetClick}) => {
        //     const newIds = Array.from(disciplineIds)
        //     tableData.lecturerIds.map(lecturerId => {
        //         tableData.lecturers[lecturerId].disciplineIds.map(disciplineId => {
        //             newIds.splice(0, 0, disciplineId)
        //         })
        //     })
        //     disciplineIds = newIds;
        //     return {disciplineIds, handleResetClick}
        // })

    //     setTableData(({disciplines, lecturerIds, lecturers}) => {
    //         const newDisciplineIds = [] as string[]
    //         lecturerIds.map(lecturerId => {
    //             lecturers[lecturerId].disciplineIds = newDisciplineIds
    //         })
    //         return {disciplines, lecturerIds, lecturers}
    //
    //     })
    // }

    // const [columnData, setColumnData] = useState<columnProps>({disciplineIds, handleResetClick})

    // const handleDndAffectingColumn = (result: DropResult) => {
    //     const destination = result.destination
    //     const source = result.source
        //dnd between disciplines in column
        // if (destination!.droppableId === source.droppableId){
        //     setColumnData(({disciplineIds, handleResetClick}) => {
        //         const destinationIds = Array.from(disciplineIds)
        //         destinationIds.splice(source.index, 1)
        //         destinationIds.splice(destination!.index, 0, result.draggableId)
        //         disciplineIds = destinationIds;
        //         return {disciplineIds,  handleResetClick}
        //     })
        // }
        // dnd from column to table
        // else if (source.droppableId === 'column'){
        //     setColumnData(({disciplineIds, handleResetClick}) => {
        //         const sourceIds = Array.from(disciplineIds)
        //         sourceIds.splice(source.index, 1)
        //         disciplineIds = sourceIds
        //         return {disciplineIds, handleResetClick}
        //     })
        //     setTableData(({lecturers, lecturerIds, disciplines}) => {
        //         const lecturer = lecturers[destination!.droppableId];
        //         const destinationIds = Array.from(lecturer.disciplineIds)
        //         destinationIds.splice(destination!.index, 0, result.draggableId)
        //         lecturers[destination!.droppableId].disciplineIds = destinationIds;
        //         return {lecturers, lecturerIds, disciplines};
        //     })
        // }
        // dnd from table to column
    //     else if (destination?.droppableId === 'column'){
    //         setColumnData(({disciplineIds, handleResetClick}) => {
    //             const destinationIds = Array.from(disciplineIds)
    //             destinationIds.splice(destination.index, 0, result.draggableId)
    //             disciplineIds = destinationIds;
    //             return {disciplineIds, handleResetClick};
    //         })
    //         setTableData(({lecturers, lecturerIds, disciplines}) => {
    //             const lecturer = lecturers[source.droppableId]
    //             const sourceIds = Array.from(lecturer.disciplineIds)
    //             sourceIds.splice(source.index, 1)
    //             lecturers[source.droppableId].disciplineIds = sourceIds;
    //             return {lecturers, lecturerIds, disciplines};
    //         })
    //     }
    // }

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
                newLecturers[lecturerIndex].disciplineIds.splice(source.index,1)
                newLecturers[lecturerIndex].disciplineIds.splice(destination!.index,0, result.draggableId)
                lecturers = newLecturers
                return lecturers
            })
        }  // another lecturer
        else if (result.source.droppableId !== destination.droppableId) {
            setLecturers(lecturers => {
                const newLecturers = Array.from(lecturers)
                const sourceLectorId = lecturers.findIndex(lecturer => lecturer.name === source.droppableId)
                const destinationLectorId = lecturers.findIndex(lecturer => lecturer.name === destination.droppableId)
                newLecturers[sourceLectorId].disciplineIds.splice(source.index, 1)
                newLecturers[destinationLectorId].disciplineIds.splice(destination.index, 0, result.draggableId)
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
            {/*<DisciplinesColumn disciplineIds={columnData.disciplineIds} handleResetClick={handleResetClick} />*/}
            <LecturersTable lecturers={lecturers} disciplines={disciplines}/>
        </DragDropContextContainer>
    </DragDropContext>)
}



const DragDropContextContainer = styled.div`
display: flex`
