import {Dispatch, FC, ReactNode, SetStateAction, useState} from "react";
import {Modal} from "./Modal";
import {GoogleSSForm} from "./GoogleSSForm";
import {Lecturer} from "../../Models/Lecturer";
import {ITablesClient} from "../../clients/TablesClient";
import {SelectImport} from "./SelectImport";

interface importModalProps{
    setLecturers: Dispatch<SetStateAction<Lecturer[]>>
    onClose: () => void
    tablesClient : ITablesClient
}

export const ImportModal : FC<importModalProps> = ({tablesClient, setLecturers , onClose}) => {
    const [importUrl, setImportUrl] = useState('')
    const [googleSSForm, setGoogleSSForm] = useState(true)
    const [xlsxForm, setXlsxForm] = useState(false)

    const handleImportSubmit = async () => {
        const regExp = new RegExp("(?<=^([^/]*/){5})([^/]*)")
        const matches = regExp.exec(importUrl)
        const spreadsheetId = matches![0];
        console.log(spreadsheetId)
        if (googleSSForm){
            await tablesClient.importTableViaLecturersTableAsync(spreadsheetId).then(response => {
                const {data} = response
                setLecturers(prevState => {
                    prevState = data
                    return prevState
                })
            })
        }
        else {
            await tablesClient.importTableViaLecturersTableAsync
        }
    }

    return(
        <Modal onClose={onClose} title={'Способ импортирования таблицы'} onSubmit={handleImportSubmit}>
            <SelectImport  xlsxForm={xlsxForm} setXlsxForm={setXlsxForm} setGoogleSSForm={setGoogleSSForm}/>
            <GoogleSSForm setUrl={setImportUrl}/>
        </Modal>)
}