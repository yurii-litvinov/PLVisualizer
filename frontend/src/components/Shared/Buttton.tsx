import styled from "styled-components";

export interface buttonProps{
    backgroundColor: string;
    color: string;
    onHoverBackgroundColor: string
}

export const Button = styled.button<buttonProps>`
  margin: 16px;
  appearance: none;
  backface-visibility: hidden;
  background-color: ${props => props.backgroundColor};
  border-radius: 32px;
  box-sizing: border-box;
  color: ${props => props.color};
  cursor: pointer;
  font-size: 16px;
  font-weight: 600;
  letter-spacing: normal;
  line-height: 1.5;
  padding: 16px 20px;
  text-align: center;
  white-space: nowrap;

  &:hover {
      background-color: ${props => props.onHoverBackgroundColor}
  }
`