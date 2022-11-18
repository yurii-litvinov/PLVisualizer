import {Modal, modalProps} from "./Modal";
import {createTablesClient, ITablesClient} from "../../clients/TablesClient";
import {FC, useState} from 'react'
import {Lecturer} from "../../Models/Lecturer";
import {GoogleSSForm} from "./GoogleSSForm";
import exp from "constants";
import {ColorRing} from "react-loader-spinner";
import styled from "styled-components";

interface exportModalProps{
    onClose: () => void
    tablesClient: ITablesClient
    lecturers: Lecturer[]
}

export const ExportModal : FC<exportModalProps> = ({tablesClient, lecturers , onClose}) => {
    const [loading, setLodaing] = useState(false)
    const [exportUrl, setExportUrl] = useState('')
    const handleExportSubmit = async () => {
        setLodaing(value => !value)
        const regExp = new RegExp("(?<=^([^/]*/){5})([^/]*)")
        const matches = regExp.exec(exportUrl)
        const spreadsheetId = matches![0];
        await tablesClient.exportTableAsync(spreadsheetId, lecturers)
        setLodaing(value => !value)
    }
    return(
    <Modal onClose={onClose} title={'Экспортирование таблицы'} onSubmit={handleExportSubmit}>
        <GoogleSSForm setUrl={setExportUrl}></GoogleSSForm>
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
    align-content: center;
  margin-left: 300px;
`