import {Dispatch, FC, SetStateAction} from "react";
import {Discipline} from "../../Models/Discipline";
import styled from "styled-components";
import {DropDownItem} from "../Shared/DropDownItem";

interface dropDownProps {
    setDisciplines: Dispatch<SetStateAction<Discipline[]>>
    setVisibility : Dispatch<SetStateAction<boolean>>
}

export const DropDown : FC<dropDownProps> = ({setDisciplines, setVisibility}) => {
    const sortByTerm = () => {
        setDisciplines( prevState => {
            return Array.from(prevState).sort((a, b) => {
                if (a.term < b.term) {
                    return -1
                }
                if (a.term > b.term) {
                    return 1
                }
                return 0
            })
        })
        setVisibility(value => !value)
    }

    const sortByProgram = () => {
        setDisciplines( prevState => {
            return Array.from(prevState).sort((a, b) => {
                if (a.educationalProgram < b.educationalProgram) {
                    return -1
                }
                if (a.educationalProgram > b.educationalProgram) {
                    return 1
                }
                return 0
            })
        })
        setVisibility(value => !value)
    }

    return <Container>
        <DropDownItem style={{width: "100%"}} onClick={sortByTerm}>По семестру</DropDownItem>
        <DropDownItem style={{width: "100%"}}  onClick={sortByProgram}>По учебной программе</DropDownItem>
    </Container>
}

const Container = styled.div`
  width: 65%;
  display: flex;
  flex-direction: column;
`

