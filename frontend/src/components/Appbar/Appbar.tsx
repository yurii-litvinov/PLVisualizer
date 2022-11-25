import React, {FC, useState} from "react";
import styled from "styled-components";
import {Actions} from "./Actions";

export interface appBarProps{
    onImportClick: () => void
    onExportClick: () => void
}

export const AppBar : FC<appBarProps> = ({onImportClick, onExportClick} : appBarProps) => {
    return(
        <AppBarContainer>
            <AppNameContainer> PLVisualizer </AppNameContainer>
            <Actions onExportClick={onExportClick} onImportClick={onImportClick}  />
        </AppBarContainer>
    )
}

const AppBarContainer = styled.nav`
  color: white;
  height: 80px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-family: "Segoe UI";
  background-image: linear-gradient(45deg, #282c34 0%, #251c34 100%);
`

const AppNameContainer = styled.div`
    padding-left: 20px;
    color: white;
    font-weight: 500;
    cursor: pointer;
    font-size: xx-large;
`

