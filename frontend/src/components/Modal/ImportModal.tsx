import {Dispatch, FC, ReactNode, SetStateAction, useState} from "react";
import {Modal} from "./Modal";
import {GoogleForm} from "./GoogleForm";
import {Lecturer} from "../../Models/Lecturer";
import {ITablesClient} from "../../clients/TablesClient";
import {SelectImport} from "./SelectImport";
import styled from "styled-components";
import {ColorRing} from "react-loader-spinner";

interface importModalProps{
    setLecturers: Dispatch<SetStateAction<Lecturer[]>>
    onClose: () => void
    tablesClient : ITablesClient
}

export const ImportModal : FC<importModalProps> = ({tablesClient, setLecturers , onClose}) => {
    const [loading, setLoading] = useState(false)
    const [importUrl, setImportUrl] = useState('')
    const [formData, setFormData] = useState<FormData>(new FormData())
    const [googleForm, setGoogleSSForm] = useState(true)
    const [xlsxForm, setXlsxForm] = useState(false)

    const handleImportSubmit = async () => {
        const regExp = new RegExp("(?<=^([^/]*/){5})([^/]*)")
        const matches = regExp.exec(importUrl)
        const spreadsheetId = matches![0];
        setLoading(value => !value)
        if (googleForm){
            await tablesClient.importTableViaLecturersTableAsync(spreadsheetId).then(response => {
                const {data} = response
                setLecturers(prevState => data)
            })
        }
        else {
            await tablesClient.importTableViaConfigAsync(spreadsheetId, formData).then(response =>{
                const {data} = response
                setLecturers(prevState => data)
                }).catch(error => console.error())
            }
        setLoading(value => !value)
        }

    return(
        <Modal onClose={onClose} title={'Способ импортирования таблицы'} onSubmit={handleImportSubmit}>
            <SelectImport  xlsxForm={xlsxForm} setXlsxForm={setXlsxForm} setGoogleSSForm={setGoogleSSForm}
                           formData={formData} setFormData={setFormData}/>
            <GoogleForm setUrl={setImportUrl}/>
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
        </Modal>)
}

const LoadingSpinnerContainer = styled.div` 
`
