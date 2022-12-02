import {Dispatch, FC, SetStateAction} from "react";
import {Discipline} from "../../Models/Discipline";
import styled from "styled-components";
import {DropDownItem} from "../Shared/DropDownItem";

interface dropDownProps {
    setDisciplines: Dispatch<SetStateAction<Array<Discipline>>>
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

    return <>
        <DropDownItem onClick={sortByTerm} 
                      style={{width: "75%"}}
                        backgroundColor={"lightgrey"}
                        onHoverBackgroundColor={"lightblue"}
                        color={"black"}>По семестру
        </DropDownItem>
        <DropDownItem onClick={sortByProgram}
                      style={{width: "75%"}}
                      backgroundColor={"lightgrey"}
                      onHoverBackgroundColor={"lightblue"}
                      color={"black"}>По учебной программе
        </DropDownItem>
    </>
}

const Container = styled.div`
  display: flex;
  flex-direction: column;
`

