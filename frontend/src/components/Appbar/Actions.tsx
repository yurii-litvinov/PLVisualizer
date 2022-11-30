import styled from 'styled-components'
import React from "react";
import {appBarProps} from "./Appbar";
import {Button} from "../Shared/Buttton";

export const Actions: React.FC<appBarProps> = ({onImportClick, onExportClick} : appBarProps) =>{
    return(
            <ActionsContainer>
                <Button backgroundColor={'white'} onHoverBackgroundColor={'lightblue'} color={'black'} onClick={onImportClick}>
                    Добавить таблицу
                </Button>
                <Button backgroundColor={'white'} onHoverBackgroundColor={'lightblue'} color={'black'}   onClick={onExportClick}>
                    Экспортировать таблицу
                </Button>
            </ActionsContainer>)
}

const ActionsContainer = styled.div`
  margin: 24px 96px 24px 24px;
  display: flex;
  width: 300px;
  justify-content: space-evenly;
  align-items: center;
`