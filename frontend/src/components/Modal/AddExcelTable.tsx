import React, {FC} from "react";
import styled from "styled-components";

export const AddExcelTable : FC = () => {
    return(
        <div>
            <input type={"file"} placeholder={'Перетащите файл сюда'}/>
        </div>
    )
}

const Container = styled.div`
  display: block;
  box-sizing: border-box;
  width: 30%;
  border-radius: 4px;
  border: 1px solid white;
  padding: 10px 15px;
  margin-bottom: 10px;
  font-size: 14px;
`