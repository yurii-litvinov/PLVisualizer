import {Modal, modalProps} from "./Modal";
import {createTablesClient, ITablesClient} from "../../clients/TablesClient";
import {FC, useState} from 'react'
import {Lecturer} from "../../Models/Lecturer";
import {AddGoogleSS} from "./AddGoogleSS";

interface exportModalProps{
    onClose: () => void
    tablesClient: ITablesClient
    lecturers: Lecturer[]
}

export const ExportModal : FC<exportModalProps> = ({tablesClient, lecturers , onClose}) => {
    const [exportUrl, setExportUrl] = useState('')
    const handleExportSubmit = async () => {
        await tablesClient.exportTable(exportUrl, lecturers)
    }
    return(
    <Modal onClose={onClose} title={'Экспортирование таблицы'} onSubmit={handleExportSubmit}>
        <AddGoogleSS setUrl={setExportUrl}></AddGoogleSS>
    </Modal>)
}