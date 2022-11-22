import React, {FC, ReactComponentElement, ReactNode} from "react";
import closeIcon from "../../img/close.svg"
import styled from "styled-components";
import {SelectImport} from "./SelectImport";

export interface modalProps{
    title: string
    children: ReactNode
    onSubmit: () => void
    onClose: () => void
}

export const Modal : FC<modalProps> = ({title, onClose, onSubmit, children}) => {
    return (
        <>
        <ModalContainer>
            <ModalOverlay>
            </ModalOverlay>
            <ModalBox>
                <CloseButton onClick={onClose}>
                    <img src={closeIcon} alt={'close modal'}></img>
                </CloseButton>
                <ModalTitle> {title} </ModalTitle>
                {children}
                <ButtonSubmit  onClick={onSubmit}>Подтвердить</ButtonSubmit>
                <ButtonCancel onClick={onClose}> Отмена </ButtonCancel>
            </ModalBox>
        </ModalContainer>
    </>
    )
}

const ModalContainer = styled.div`
    position: fixed;
    top: 0;
    left: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100vw;
    height: 100vh;
    font-family: "Segoe UI";
`

const ModalBox = styled.div`
    position:relative;
    width: 50%;
    margin: 0 10%;
    padding: 50px;
    box-sizing: border-box;
    border-radius: 10px;
    background-color: white;
    cursor: auto;
    font-family: inherit;
  align-items: center;
`

const ModalTitle = styled.div`
  margin: 20px;
    color: black;
    font-size: 30px;
    font-family: inherit
`

const CloseButton = styled.div`
    position: absolute;
    top: 20px;
    right: 20px;
    transition: transform 250ms ease-in-out;
    transform-origin: 50% 50%;
    cursor: pointer;
    font-family: inherit;
`

const ModalOverlay = styled.div`
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, .8);
    cursor: pointer;
    font-family: inherit
`

const ButtonSubmit = styled.button`
  padding: 0.8rem 1.2rem;
  border-style: none;
  border-radius: 9999px;
  background-color: black;
  box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.15);
  font-size: 1rem;
  font-weight: 600;
  color: white;
  cursor: pointer;
  outline: none;
`

const ButtonCancel = styled.button`
  margin: 10px;
  padding: 0.8rem 1.2rem;
  border-style: none;
  border-radius: 9999px;
  background-color: lightgray;
  box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.15);
  font-size: 1rem;
  font-weight: 600;
  color: black;
  cursor: pointer;
  outline: none;
`