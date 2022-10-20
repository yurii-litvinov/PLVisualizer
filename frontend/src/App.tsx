import React, {FC, useState} from 'react';
import './App.css';
import {AppBar} from "./components/Appbar/Appbar";
import {Modal} from "./components/Modal/Modal";
import {SelectImport} from "./components/Modal/SelectImport";
import {DragDropRegion} from "./components/DragDropRegion/DragDropRegion";
import {AddGoogleSS} from "./components/Modal/AddGoogleSS";
import {Lecturer} from "./Models/Lecturer"
import {Discipline} from "./Models/Discipline";
import {createTablesClient} from "./clients/TablesClient";


function App() {
    const [importModal, setImportModal] = useState(false)
    const [exportModal, setExportModal] = useState(false)
    const toggleImportModal = () => setImportModal(value => !value)
    const toggleExportModal = () => setExportModal(value => !value)

    const [exportTableUrl, setExportTableUrl] = useState('')
    const [importTableUrl, setImportTableUrl] = useState('')

    const [lecturers, setLecturers] = useState([] as Lecturer[])
    const [disciplines, setDisciplines] = useState({} as {[key:string]:Discipline})

    const tablesClient = createTablesClient()

    const handleExportSubmit = async () => {
        await tablesClient.exportTable(exportTableUrl, lecturers)
    }

    const handleImportSubmit = async () => {
        await tablesClient.importTable(importTableUrl).then(response => {
            const {data} = response
            setLecturers(prevState => {
                prevState = data
                return prevState
            })
        })
    }

    return(<div className="Appbar">
            <AppBar onImportClick={toggleImportModal} onExportClick={toggleExportModal}></AppBar>
            { importModal && <Modal
                title={'Добавление таблицы'}
                onClose={toggleImportModal}
                onSubmit={handleImportSubmit}>
                <SelectImport onCancelClick={toggleImportModal}/>
            </Modal>
            }
            { exportModal && <Modal
                title={'Добавление таблицы'}
                onClose={toggleExportModal}
                onSubmit={handleExportSubmit}>
                <AddGoogleSS/>
            </Modal>
            }
            <DragDropRegion  lecturers={lecturers} disciplines={disciplines} setLecturers={setLecturers}/>
    </div>
    )
}

export default App;
