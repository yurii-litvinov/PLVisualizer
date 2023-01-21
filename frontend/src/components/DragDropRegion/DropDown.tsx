import {Dispatch, FC, SetStateAction, useState} from "react";
import {Discipline} from "../../Models/Discipline";
import {DropDownItem} from "../Shared/DropDownItem";

interface dropDownProps {
    setDisciplines: Dispatch<SetStateAction<Array<Discipline>>>
}

export const DropDown : FC<dropDownProps> = ({setDisciplines}) => {
    const [dropDown, setDropDown] = useState(false)
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
        setDropDown(value => !value)
    }

    // const sortByProgram = () => {
    //     setDisciplines( prevState => {
    //         return Array.from(prevState).sort((a, b) => {
    //             if (a.educationalProgram < b.educationalProgram) {
    //                 return -1
    //             }
    //             if (a.educationalProgram > b.educationalProgram) {
    //                 return 1
    //             }
    //             return 0
    //         })
    //     })
    //     setDropDown(value => !value)
    // }

    return <>
        <DropDownItem
            onClick={() => setDropDown(value => !value)}
            backgroundColor={"white"}
            onHoverBackgroundColor={"lightblue"}
            color = "black">
            Сортировка
        </DropDownItem>
        {dropDown &&
            <>
        <DropDownItem onClick={sortByTerm} 
                      style={{width: "75%"}}
                        backgroundColor={"white"}
                        onHoverBackgroundColor={"lightblue"}
                        color={"black"}>По семестру
        </DropDownItem>
        {/* <DropDownItem onClick={sortByProgram}
                      style={{width: "75%"}}
                      backgroundColor={"white"}
                      onHoverBackgroundColor={"lightblue"}
                      color={"black"}>По учебной программе
        </DropDownItem> */}
        </>
        }
    </>
}


