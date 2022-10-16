import React, {FC} from "react";
import styled from "styled-components";

export const AddGoogleSS : FC = () =>{
    return(
    <Container>
        <InputContainer type={"text"}  placeholder={'Ссылка на Google Spreadsheet таблицу'}/>
    </Container>)
}

const Container = styled.div`
  box-sizing: border-box;
  width: 50%;
  border-radius: 4px;
  border: 1px solid white;
  padding: 10px 15px;
  margin-bottom: 10px;
  font-size: 14px;
`

const InputContainer = styled.input`
  box-sizing: border-box;
  width: 400px;
  border-radius: 4px;
  border: 1px solid skyblue;
  padding: 10px 15px;
  margin: 10px;
  font-size: 14px;
`