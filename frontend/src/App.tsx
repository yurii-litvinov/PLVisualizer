import React, {FC, useState} from 'react';
import './App.css';
import {AppBar} from "./components/Appbar/Appbar";
import {Modal} from "./components/Modal/Modal";
import {SelectImport} from "./components/Modal/SelectImport";
import {dndData} from "./components/data";
import {DisciplinesTable} from "./components/DragDropRegion/DisciplinesTable";
import {DragDropRegion} from "./components/DragDropRegion/DragDropRegion";

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
            <DragDropRegion tableProps={dndData} columnProps={dndData} />
    </div>
    )
}

export default App;
