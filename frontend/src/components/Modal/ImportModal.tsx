import {Dispatch, FC, SetStateAction, useEffect, useState} from "react";
import {Modal} from "./Modal";
import {GoogleForm} from "./GoogleForm";
import {Lecturer} from "../../Models/Lecturer";
import {ITablesClient} from "../../clients/TablesClient";
import {SelectImport} from "./SelectImport";
import styled from "styled-components";
import {ColorRing} from "react-loader-spinner";
import {Response} from "../../Models/Response";

interface importModalProps{
    setLecturers: Dispatch<SetStateAction<Lecturer[]>>
    closeModal: () => void
    tablesClient : ITablesClient
}


export const ImportModal : FC<importModalProps> = ({tablesClient, setLecturers , closeModal}) => {
    const [loading, setLoading] = useState(false)
    const [importUrl, setImportUrl] = useState('')
    const [formData, setFormData] = useState<FormData>(new FormData())
    const [googleForm, setGoogleSSForm] = useState(false)
    const [excelForm, setExcelForm] = useState(true)
    const [errorMessage, setErrorMessage] = useState("")
    // useEffect(() => {
    //     if (errorMessage === ""){
    //         closeModal()
    //     }
    // }, [errorMessage])

    const handleImportSubmit = async () => {
        const regExp = new RegExp("(?<=^([^/]*/){5})([^/]*)")
        const matches = regExp.exec(importUrl)
        const spreadsheetId = matches![0];
        setLoading(value => !value)
        if (googleForm){
            await tablesClient.importTableViaLecturersTableAsync(spreadsheetId)
                .then(response => {
                const {data} = response
                setLecturers(() => data)
            })
                .catch(function (error) {
                    if (error.response) {
                        const jsonResponse =  JSON.stringify(error.response.data)
                        const response : Response = JSON.parse(jsonResponse)
                        setErrorMessage(response.Content)
                    }})
        }
        else {
            await tablesClient.importTableViaConfigAsync(spreadsheetId, formData).then(response =>{
                const {data} = response
                setLecturers(prevState => data)
                }).catch(function (error){
                const jsonResponse =  JSON.stringify(error.response.data)
                const response : Response = JSON.parse(jsonResponse)
                setErrorMessage(prevState => response.Content)
                })
        }

        setLoading(value => !value)
        // if (errorMessage === "") closeModal()
        }

    return(
        <Modal onClose={closeModal} title={'Способ импортирования таблицы'} onSubmit={handleImportSubmit}>
            <SelectImport  xlsxForm={excelForm} setXlsxForm={setExcelForm} setGoogleSSForm={setGoogleSSForm}
                           formData={formData} setFormData={setFormData}/>
            <GoogleForm setUrl={setImportUrl} placeholder={excelForm ?
                'Ссылка на конфигурационную Google Spreadsheet таблицу' :
            'Ссылка на Google Spreadsheet таблицу с преподавателями'}/>
            {loading && <LoadingSpinnerContainer>
                <ColorRing
                    visible={true}
                    height="80"
                    width="80"
                    margin-left='100px'
                    ariaLabel="blocks-loading"
                    wrapperStyle={{}}
                    wrapperClass="blocks-wrapper"
                    colors={['#e15b64', '#f47e60', '#f8b26a', '#abbd81', '#849b87']}
                />
            </LoadingSpinnerContainer>}
            {errorMessage !== "" && <h3 style={{marginLeft: "12px"}}>{errorMessage}</h3>}
        </Modal>)
}

const LoadingSpinnerContainer = styled.div` 
`
