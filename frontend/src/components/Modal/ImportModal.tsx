import {Dispatch, FC, ReactNode, SetStateAction, useState} from "react";
import {Modal} from "./Modal";
import {AddGoogleSS} from "./AddGoogleSS";
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

    const handleImportSubmit = async () => {
        await tablesClient.importTable(importUrl).then(response => {
            const {data} = response
            setLecturers(prevState => {
                prevState = data
                return prevState
            })
        })
    }

    return(
        <Modal onClose={onClose} title={'Способ импортирования таблицы'} onSubmit={handleImportSubmit}>
            <SelectImport setImportUrl={setImportUrl}/>
        </Modal>)
}