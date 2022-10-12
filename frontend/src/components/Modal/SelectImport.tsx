import React, {FC, useState} from "react";
import {AddGoogleSS} from "./AddGoogleSS";
import {AddExcelTable} from "./AddExcelTable";
import {FormControlLabel, RadioGroup, Radio} from "@material-ui/core";

export const SelectImport : FC = () => {
    const [googleSSForm, setGoogleSSForm] = useState(true)
    const [excelTableForm, setExcelTableForm] = useState(false)

    const toggleForm = () => {
        setGoogleSSForm(value => !value)
        setExcelTableForm(value => !value)
    }

    return(
        <>
        <fieldset>
            <legend>Выберите способ импортирования таблицы</legend>
                <RadioGroup onChange={toggleForm}>
                    <FormControlLabel control={<Radio />} label={'Google-spreadsheet таблица'} value={'google-spreadsheet таблица'} />
                    <FormControlLabel control={<Radio />} label={'Excel таблица'} value={'excel таблица'}  />
                </RadioGroup>
        </fieldset>
    {googleSSForm && <AddGoogleSS/>}
    {excelTableForm && <AddExcelTable/>}
        </>
    )
}