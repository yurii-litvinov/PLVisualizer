import React, {FC} from "react";
import styled from "styled-components";
import {Actions} from "./Actions";

export interface appBarProps{
    onImportClick: () => void
    onExportClick: () => void
    onHelpClick: () => void
}

export const AppBar : FC<appBarProps> = ({onImportClick, onExportClick, onHelpClick} : appBarProps) => {
    return(
        <AppBarContainer>
            <AppNameContainer> Визуализатор педнагрузки </AppNameContainer>
            <Actions onExportClick={onExportClick} onImportClick={onImportClick}  onHelpClick={onHelpClick} />
        </AppBarContainer>
    )
}

const AppBarContainer = styled.nav`
  position: fixed;
  color: white;
  height: 5%;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-image: linear-gradient(45deg, #282c34 0%, #251c34 100%);
`

const AppNameContainer = styled.div`
    position: fixed;
    padding-left: 20px;
    color: white;
    font-weight: 500;
    cursor: pointer;
    font-size: xx-large;
`

