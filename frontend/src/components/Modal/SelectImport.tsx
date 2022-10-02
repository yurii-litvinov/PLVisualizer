import React, {FC, useState} from "react";
import {AddGoogleSS} from "./AddGoogleSS";
import {AddExcelTable} from "./AddExcelTable";

export const SelectImport : FC = () => {
    const [googleSSForm, setGoogleSSForm] = useState(true)
    const [excelTableForm, setExcelTableForm] = useState(false)

    const toggleForm = () => {
        setGoogleSSForm(value => !value)
        setExcelTableForm(value => !value)
    }

    return(
        <>
            <select>
                <option onClick={toggleForm}>Google-spreadsheet таблица</option>
                <option onClick={toggleForm}>Excel файл</option>
            </select>
            {googleSSForm && <AddGoogleSS/>}
            {excelTableForm && <AddExcelTable/>}
        </>
    )
}