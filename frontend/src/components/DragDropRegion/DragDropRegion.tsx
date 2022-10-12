import {FC, useState} from "react";
import {DragDropContext, DropResult} from "react-beautiful-dnd";
import {columnProps, DisciplinesColumn} from "./DisciplinesColumn";
import {DisciplinesTable, tableProps} from "./DisciplinesTable";


interface dragDropRegionProps {
    tableProps : tableProps;
    columnProps: columnProps
}

export const DragDropRegion : FC<dragDropRegionProps>  = ({tableProps, columnProps}) => {
    const [tableData, setTableData] = useState(tableProps);
    const [columnData, setColumnData] = useState(columnProps)

    const handleDndAffectingColumn = (result: DropResult) => {
        const destination = result.destination
        const source = result.source
        //dnd between disciplines in column
        if (destination!.droppableId === source.droppableId){
            setColumnData(({disciplineIds}) => {
                const destinationIds = Array.from(disciplineIds)
                destinationIds.splice(source.index, 1)
                destinationIds.splice(destination!.index, 0, result.draggableId)
                disciplineIds = destinationIds;
                return {disciplineIds}
            })
        }
        // dnd from column to table
        else if (source.droppableId === 'column'){
            setColumnData(({disciplineIds}) => {
                const sourceIds = Array.from(disciplineIds)
                sourceIds.splice(source.index, 1)
                disciplineIds = sourceIds
                return {disciplineIds}
            })
            setTableData(({lecturers, lecturerIds, disciplines}) => {
                const lecturer = lecturers[destination!.droppableId];
                const destinationIds = Array.from(lecturer.disciplineIds)
                destinationIds.splice(destination!.index, 0, result.draggableId)
                lecturers[destination!.droppableId].disciplineIds = destinationIds;
                return {lecturers, lecturerIds, disciplines};
            })
        }
        // dnd from table to column
        else if (destination?.droppableId === 'column'){
            setColumnData(({disciplineIds}) => {
                const destinationIds = Array.from(disciplineIds)
                destinationIds.splice(destination.index, 0, result.draggableId)
                disciplineIds = destinationIds;
                return {disciplineIds};
            })
            setTableData(({lecturers, lecturerIds, disciplines}) => {
                const lecturer = lecturers[source.droppableId]
                const sourceIds = Array.from(lecturer.disciplineIds)
                sourceIds.splice(source.index, 1)
                lecturers[source.droppableId].disciplineIds = sourceIds;
                return {lecturers, lecturerIds, disciplines};
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
            setTableData(({lecturers, lecturerIds, disciplines}) => {
                const lecturer = lecturers[source.droppableId]
                const destinationIds = Array.from(lecturer.disciplineIds)
                destinationIds.splice(source.index,1)
                destinationIds.splice(destination!.index,0, result.draggableId)
                lecturers[source.droppableId].disciplineIds = destinationIds;
                return {lecturers, lecturerIds, disciplines}
            })
        }  // another lecturer
        else if (result.source.droppableId !== destination.droppableId) {
            setTableData(({lecturers, lecturerIds, disciplines}) => {
                const sourceIds = Array.from(lecturers[source.droppableId].disciplineIds)
                sourceIds.splice(result.source.index, 1)
                const destinationIds = Array.from(lecturers[destination!.droppableId].disciplineIds)
                destinationIds.splice(destination!.index, 0, result.draggableId)
                lecturers[destination!.droppableId].disciplineIds = destinationIds;
                lecturers[source.droppableId].disciplineIds = sourceIds
                return {lecturers, lecturerIds, disciplines}
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
        if (destination?.droppableId === 'column' || source.droppableId === 'column'){
            handleDndAffectingColumn(result)
        }
        else {
            handleTableDnd(result)
        }
    }

    return(
    <DragDropContext onDragEnd={handleDragEnd}>
        <DisciplinesColumn disciplineIds={columnData.disciplineIds} />
        <DisciplinesTable lecturerIds={tableData.lecturerIds} lecturers={tableData.lecturers} disciplines={tableData.disciplines}/>
    </DragDropContext>)
}
