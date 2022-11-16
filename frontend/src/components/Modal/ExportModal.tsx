import {Modal, modalProps} from "./Modal";
import {createTablesClient, ITablesClient} from "../../clients/TablesClient";
import {FC, useState} from 'react'
import {Lecturer} from "../../Models/Lecturer";
import {GoogleSSForm} from "./GoogleSSForm";
import exp from "constants";

interface exportModalProps{
    onClose: () => void
    tablesClient: ITablesClient
    lecturers: Lecturer[]
}

export const ExportModal : FC<exportModalProps> = ({tablesClient, lecturers , onClose}) => {
    const [exportUrl, setExportUrl] = useState('')
    const handleExportSubmit = async () => {
        const regExp = new RegExp("(?<=^([^/]*/){5})([^/]*)")
        const matches = regExp.exec(exportUrl)
        const spreadsheetId = matches![0];
        await tablesClient.exportTableAsync(spreadsheetId, lecturers)
    }
    return(
    <Modal onClose={onClose} title={'Экспортирование таблицы'} onSubmit={handleExportSubmit}>
        <GoogleSSForm setUrl={setExportUrl}></GoogleSSForm>
    </Modal>)
}