import React, {FC, useState} from 'react';
import './App.css';
import {DragDropContext} from 'react-beautiful-dnd'
import {AppBar} from "./components/Appbar/Appbar";
import {Modal} from "./components/Modal/Modal";
import {SelectImport} from "./components/Modal/SelectImport";
import {tableData} from "./components/data";
import styled from "styled-components"
import {DisciplinesTable} from "./components/DisciplinesTable";

function App() {
    const [importModal, setImportModal] = useState(false)
    const [exportModal, setExportModal] = useState(false)
    const toggleImportModal = () => setImportModal(value => !value)
    const toggleExportModal = () => setExportModal(value => !value)
    return(<div className="Appbar">
            <AppBar onImportClick={toggleImportModal} onExportClick={toggleExportModal}></AppBar>
            { importModal && <Modal
                title={'Добавьте таблицу'}
                onClose={toggleImportModal}
            > <SelectImport/>
            </Modal>}
            { exportModal && <Modal
                title={''}
                onClose={toggleExportModal}
            >
            </Modal>}
            <DisciplinesTable items={tableData}/>
    </div>
    )
}

export default App;
