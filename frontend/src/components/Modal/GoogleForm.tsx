import React, {Dispatch, FC, FormEvent} from "react";
import styled from "styled-components";

interface googleFormProps {
    setUrl: Dispatch<React.SetStateAction<string>>
    placeholder : string
}

export const GoogleForm : FC<googleFormProps> = ({setUrl, placeholder}) =>{
    const handleInputChange = (e : FormEvent<HTMLInputElement>) => {
        const newUrl = e.currentTarget.value
        setUrl( () => newUrl)
    }

    return(
    <Container>
        <InputContainer type={"text"}  placeholder={placeholder}  onInput={handleInputChange}/>
    </Container>)
}

const Container = styled.div`
  margin-left: 16px;
  box-sizing: border-box;
  width: 100%;
  border-radius: 4px;
  border: 1px solid white;
  padding: 10px 15px;
  margin-bottom: 10px;
  font-size: 14px;
`

const InputContainer = styled.input`
  box-sizing: border-box;
  width: 100%;
  border-radius: 4px;
  margin-left: -16px;
  border: 2px solid skyblue;
  padding: 10px 15px;
  font-size: 14px;
`