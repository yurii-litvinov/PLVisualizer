import React, {FC, useState} from "react";
import {AddGoogleSS} from "./AddGoogleSS";
import {AddXlsxTable} from "./AddXlsxTable";
import {FormControlLabel, RadioGroup, Radio} from "@material-ui/core";
import styled from "styled-components";

interface selectImportProps{
    onCancelClick : () => void
}

export const SelectImport : FC<selectImportProps> = ({onCancelClick}) => {
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
            <ButtonSubmit>Подтвердить</ButtonSubmit>
            <ButtonCancel onClick={onCancelClick}> Отмена </ButtonCancel>
        </>
    )
}

const ButtonSubmit = styled.button`
  padding: 0.8rem 1.2rem;
  border-style: none;
  border-radius: 9999px;
  background-color: black;
  box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.15);
  font-size: 1rem;
  font-weight: 600;
  color: white;
  cursor: pointer;
  outline: none;
`

const ButtonCancel = styled.button`
  margin: 10px;
  padding: 0.8rem 1.2rem;
  border-style: none;
  border-radius: 9999px;
  background-color: lightgray;
  box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.15);
  font-size: 1rem;
  font-weight: 600;
  color: black;
  cursor: pointer;
  outline: none;
`