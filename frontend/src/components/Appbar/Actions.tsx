import styled from 'styled-components'
import React from "react";
import {appBarProps} from "./Appbar";

const ActionsContainer = styled.ul`
    margin-right: 100px;
    display: flex;
    width: 300px;
    justify-content: space-evenly;
    align-items: center;
`

const ButtonContainer = styled.button`

  appearance: none;
  backface-visibility: hidden;
  background-color: white;
  border-radius: 8px;
  border-style: none;
  box-shadow: rgba(39, 174, 96, .15) 0 4px 9px;
  box-sizing: border-box;
  color: black;
  cursor: pointer;
  display: inline-block;
  font-family: "Segoe UI";
  font-size: 16px;
  font-weight: 600;
  letter-spacing: normal;
  line-height: 1.5;
  outline: none;
  overflow: visible ;
  padding: 13px 20px;
  position: relative;
  text-align: center;
  text-decoration: none;
  transform: translate3d(0, 0, 0);
  transition: all .3s;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
  vertical-align: top;
  white-space: nowrap;
`


export const Actions: React.FC<appBarProps> = ({onImportClick, onExportClick} : appBarProps) =>{
    return(
            <ActionsContainer>
                < ButtonContainer onClick={onImportClick}>Добавить таблицу </ButtonContainer>
                <ButtonContainer  onClick={onExportClick}>Экспортировать таблицу</ButtonContainer>
            </ActionsContainer>)
}