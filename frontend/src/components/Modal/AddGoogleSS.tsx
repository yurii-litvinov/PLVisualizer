import React, {FC} from "react";
import styled from "styled-components";

export const AddGoogleSS : FC = () =>{
    return(
    <Container>
        <input type={"text"}  placeholder={'Ссылка на Google Spreadsheet таблицу'}/>
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