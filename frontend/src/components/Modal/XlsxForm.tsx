import React, {ChangeEvent, Dispatch, FC, SetStateAction, useRef, useState} from "react";
import styled from "styled-components";
import {Button} from "../Shared/Buttton";

 interface xlsxFormProps{
     setFormData : Dispatch<SetStateAction<FormData>>
     formData: FormData
}

export const XlsxForm : FC<xlsxFormProps> = ({formData, setFormData}) => {
     const inputRef = useRef<HTMLInputElement>(null)
     const [fileName, setFileName] = useState('')

     const handleFileChange = (e : ChangeEvent<HTMLInputElement>) =>{
         e.preventDefault()
         setFormData( prevState =>  {
             const file = e.target.files![0]
             setFileName( () => file.name)
             const formData = new FormData()
             formData.append('file', file)
             return formData
         })
     }

    return(
        <DropZone onDragOver={(event) => event.preventDefault()}
        onDrop={(event) => {
            event.preventDefault()
            setFormData((prevState) =>{
                const file = event.dataTransfer!.files[0]
                setFileName( () => file.name)
                const formData = new FormData()
                formData.append('file', file)
                return formData
        })}}>
            <div> Перетащите файл сюда </div>
            <div> Или </div>
            {fileName !== ''&& <div> Выбран файл {fileName} </div>}
            <input type={"file"}
                   hidden
                   ref={inputRef}
                   onChange={handleFileChange} />
            <Button onHoverBackgroundColor={"darkblue"}
                    backgroundColor={"blue"}
                    color={'white'}
                    onClick={() => inputRef.current!.click()} >Выбрать файл</Button>
        </DropZone>
    )
}

const DropZone = styled.div`
  padding: 8px;
  justify-content: center;
  align-items: center;
  margin: 8px;
  display: flex;
  box-sizing: border-box;
  width: 98%;
  height: 150px;
  border-radius: 4px;
  border: 2px dashed;
  font-size: 14px;
  flex-direction: column;
  &:hover{
    background-color: skyblue;
  }
`

