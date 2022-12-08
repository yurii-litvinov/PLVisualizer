import styled from 'styled-components'
import React from "react";
import {appBarProps} from "./Appbar";
import {Button} from "../Shared/Buttton";

export const Actions: React.FC<appBarProps> = ({onImportClick, onExportClick} : appBarProps) =>{
    return(
            <ActionsContainer>
                <Button style={{height:"90%"}}
                        backgroundColor={'white'}
                        onHoverBackgroundColor={'lightblue'}
                        color={'black'}
                        onClick={onImportClick}>
                    Добавить таблицу
                </Button>
                <Button style={{height:"90%"}}
                        backgroundColor={'white'}
                        onHoverBackgroundColor={'lightblue'}
                        color={'black'}
                        onClick={onExportClick}>
                    Экспортировать таблицу
                </Button>
            </ActionsContainer>)
}

const ActionsContainer = styled.div`
  position: fixed;
  margin-left: 75%;
  display: flex;
  width: 15%;
  justify-content: space-evenly;
  align-items: center;
`