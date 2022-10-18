import React, {FC, useState} from "react";
import {AddGoogleSS} from "./AddGoogleSS";
import {AddXlsxTable} from "./AddXlsxTable";
import {FormControlLabel, RadioGroup, Radio} from "@material-ui/core";
import styled from "styled-components";

interface selectImportProps{
    onCancelClick : () => void
}

export const SelectImport : FC<selectImportProps> = () => {
    const [googleSSForm, setGoogleSSForm] = useState(true)
    const [excelTableForm, setExcelTableForm] = useState(false)

    const toggleForm = () => {
        setGoogleSSForm(value => !value)
        setExcelTableForm(value => !value)
    }

    return(
        <>
        <fieldset>
            <legend>Способ импортирования таблицы</legend>
                <RadioGroup onChange={toggleForm} defaultValue={'google spreadsheet таблица'}>
                    <FormControlLabel control={<Radio />} label={'Google spreadsheet таблица'} value={'google spreadsheet таблица'} />
                    <FormControlLabel control={<Radio />} label={'Xlsx файл'} value={'xlsx файл'}  />
                </RadioGroup>
        </fieldset>
        {googleSSForm && <AddGoogleSS/>}
         {excelTableForm && <AddXlsxTable/>}
        </>
    )
}
