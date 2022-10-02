import React, {FC, useState} from 'react';
import './App.css';
import {AppBar} from "./components/Appbar/Appbar";
import {Modal} from "./components/Modal/Modal";
import {AddExcelTable} from "./components/Modal/AddExcelTable";
import {AddGoogleSS} from "./components/Modal/AddGoogleSS";
import {SelectImport} from "./components/Modal/SelectImport";


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

    </div>
    )
}

export default App;
