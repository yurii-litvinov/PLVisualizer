import React, {FC, useState} from 'react';
import './App.css';
import {AppBar} from "./components/Appbar/Appbar";
import {AddTableModal} from "./components/Modal/AddTableModal";
import {SelectImport} from "./components/Modal/SelectImport";
import {dndData} from "./components/data";
import {DragDropRegion} from "./components/DragDropRegion/DragDropRegion";

function App() {
    const [importModal, setImportModal] = useState(false)
    const [exportModal, setExportModal] = useState(false)
    const toggleImportModal = () => setImportModal(value => !value)
    const toggleExportModal = () => setExportModal(value => !value)
    return(<div className="Appbar">
            <AppBar onImportClick={toggleImportModal} onExportClick={toggleExportModal}></AppBar>
            { importModal && <AddTableModal
                title={'Добавление таблицы'}
                onClose={toggleImportModal}
            />}
            <DragDropRegion disciplineIds={dndData.disciplineIds} lecturerIds={dndData.lecturerIds}
                lecturers={dndData.lecturers} disciplines={dndData.disciplines}/>
    </div>
    )
}

export default App;
