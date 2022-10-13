import React, {FC} from "react";
import styled from "styled-components";

export const AddExcelTable : FC = () => {
    return(
        <>
        <InputContainer type={"file"} placeholder={'Перетащите файл сюда'}/>
        <DropZone placeholder={'Или перетащите файл сюда'}>

        </DropZone>
        </>
    )
}

const InputContainer = styled.input`
  display: block;
  box-sizing: border-box;
  width: 30%;
  border-radius: 4px;
  border: 1px solid white;
  padding: 10px 15px;
  margin-bottom: 10px;
  font-size: 14px;
`

const DropZone = styled.div`
  display: flex;
  box-sizing: border-box;
  width: 100%;
  height: 150px;
  border-radius: 4px;
  border: 1px solid black;
  padding: 10px 15px;
  margin-bottom: 10px;
  font-size: 14px;
`