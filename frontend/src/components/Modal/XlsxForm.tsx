import React, {ChangeEvent, Dispatch, FC, FormEvent, SetStateAction} from "react";
import styled from "styled-components";
import {GoogleForm} from "./GoogleForm";

 interface xlsxFormProps{
    setFormData : Dispatch<SetStateAction<FormData>>
}

export const XlsxForm : FC<xlsxFormProps> = ({setFormData}) => {
     const handleFileChange = (e : ChangeEvent<HTMLInputElement>) =>{
         e.preventDefault()
         setFormData( prevState =>  {
             const file = e.target.files![0]
             const formData = new FormData();
             formData.append('file', file)
             return formData
         })
     }

    return(
        <>
        <DropZone type={"file"} id={"xlsxFile"} placeholder={'Или перетащите файл сюда'} onChange={handleFileChange}>
        </DropZone>
        </>
    )
}

const DropZone = styled.input`
  margin: 10px;
  display: flex;
  box-sizing: border-box;
  width: 100%;
  height: 150px;
  border-radius: 4px;
  border: 3px dashed;
  padding: 10px 15px;
  font-size: 14px;
`