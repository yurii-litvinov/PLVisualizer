import React, {Dispatch, FC, SetStateAction, useState} from "react";
import {GoogleSSForm} from "./GoogleSSForm";
import {XlsxForm} from "./XlsxForm";
import {FormControlLabel, RadioGroup, Radio} from "@material-ui/core";
import styled from "styled-components";

interface selectImportProps{
    setImportUrl : Dispatch<React.SetStateAction<string>>
    googleSSForm: boolean
    setGoogleSSForm : Dispatch<SetStateAction<boolean>>
    xlsxForm: boolean
    setXlsxForm : Dispatch<SetStateAction<boolean>>
}

export const SelectImport : FC<selectImportProps> = ({setImportUrl, setGoogleSSForm, googleSSForm, setXlsxForm, xlsxForm}) => {

    const toggleForm = () => {
        setGoogleSSForm(value => !value)
        setXlsxForm(value => !value)
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
        {googleSSForm && <GoogleSSForm setUrl={setImportUrl}/>}
         {xlsxForm && <XlsxForm/>}
        </>
    )
}
