import styled from "styled-components";

export interface dropwDownItemProps {
    backgroundColor: string;
    color: string;
    onHoverBackgroundColor: string
}


export const DropDownItem = styled.div<dropwDownItemProps>`
  appearance: none;
  backface-visibility: hidden;
  width: 75%;
  height: 60px;
  background-color: ${props => props.backgroundColor};
  border-radius: 4px;
  box-sizing: border-box;
  color: ${props => props.color};
  cursor: pointer;
  font-family: "Segoe UI";
  font-size: 16px;
  font-weight: 600;
  letter-spacing: normal;
  line-height: 1.5;
  padding: 16px 20px;
  text-align: center;
  white-space: nowrap;
  border: 1px solid grey;
  &:hover {
      background-color: ${props => props.onHoverBackgroundColor}
  }
`