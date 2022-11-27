import React, {Dispatch, FC, SetStateAction, useState} from "react";
import {GoogleForm} from "./GoogleForm";
import {XlsxForm} from "./XlsxForm";
import {FormControlLabel, RadioGroup, Radio} from "@mui/material";
import styled from "styled-components";

interface selectImportProps{
    setGoogleSSForm : Dispatch<SetStateAction<boolean>>
    xlsxForm: boolean
    setXlsxForm : Dispatch<SetStateAction<boolean>>
    setFormData : Dispatch<SetStateAction<FormData>>
}

export const SelectImport : FC<selectImportProps> = ({setGoogleSSForm, setXlsxForm, xlsxForm, setFormData}) => {

    const toggleForm = () => {
        setGoogleSSForm(value => !value)
        setXlsxForm(value => !value)
    }

    return(
        <Container>
        <fieldset>
            <legend>Способ импортирования таблицы</legend>
                <RadioGroup onChange={toggleForm} defaultValue={'google spreadsheet таблица'}>
                    <FormControlLabel control={<Radio />} label={'Google Spreadsheet таблица'} value={'google spreadsheet таблица'} />
                    <FormControlLabel control={<Radio />} label={'.xlsx файл'} value={'xlsx файл'}  />
                </RadioGroup>
        </fieldset>
         {xlsxForm && <XlsxForm setFormData={setFormData}/>}
        </Container>
    )
}

const Container = styled.div`
  margin-left: 16px`
