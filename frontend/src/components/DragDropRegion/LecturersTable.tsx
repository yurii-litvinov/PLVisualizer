import React, {FC} from 'react'
import {Lecturer} from "../../Models/Lecturer";
import { LecturerRow } from "./LecturerRow";
import styled from "styled-components";

export interface TableProps {
    lecturers: Array<Lecturer>
}

/// Represents a table with a pedagogical load with the possibility of Drag&Drop
export const LecturersTable : FC<TableProps> = ({lecturers}) => {
    return(
        <TableContainer>
            <TableRow>
                <TableHeader style={{width: "1.5%"}}></TableHeader>
                <TableHeader style={{width: "13%"}}>ФИО</TableHeader>
                <TableHeader style={{width: "10%"}}>Должность</TableHeader>
                <TableHeader style={{width: "10%"}}>Процент ставки</TableHeader>
                <TableHeader style={{width: "40%"}}>Дисциплины</TableHeader>
                <TableHeader style={{width: "15%"}}>Распределенная нагрузка</TableHeader>
                <TableHeader style={{width: "5%"}}>Ожидаемая нагрузка</TableHeader>
            </TableRow>
            {lecturers.map(lecturer => <LecturerRow lecturer={lecturer} />)}

        </TableContainer>
    )
}

const TableHeader = styled.div`
  margin-left: 8px;
  font-weight: 600;
  display: flex;
  flex-flow: row wrap;
  transition: 0.5s`

const TableRow = styled.div`
  display: flex;
  box-sizing: border-box;
  flex-direction: row;
  flex-wrap: wrap;
  border: 1px solid lightblue;
`

const TableContainer = styled.div`
  margin-top: 3%;
  margin-left: 15.5%;
  width: 100%;
  display: flex;
  flex-direction: column`


