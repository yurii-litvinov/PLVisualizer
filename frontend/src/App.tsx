import React, {FC, useState} from 'react';
import './App.css';
import {AppBar} from "./components/Appbar/Appbar";
import {Modal} from "./components/Modal/Modal";
import {SelectImport} from "./components/Modal/SelectImport";
import {dndData} from "./components/data";
import {DragDropRegion} from "./components/DragDropRegion/DragDropRegion";
import {AddGoogleSS} from "./components/Modal/AddGoogleSS";

function App() {
    const [importModal, setImportModal] = useState(false)
    const [exportModal, setExportModal] = useState(false)
    const toggleImportModal = () => setImportModal(value => !value)
    const toggleExportModal = () => setExportModal(value => !value)
    return(<div className="Appbar">
            <AppBar onImportClick={toggleImportModal} onExportClick={toggleExportModal}></AppBar>
            { importModal && <Modal
                title={'Добавление таблицы'}
                onClose={toggleImportModal}>
                <SelectImport onCancelClick={toggleImportModal}/>
            </Modal>
            }
            { exportModal && <Modal
                title={'Добавление таблицы'}
                onClose={toggleExportModal}>
                <AddGoogleSS/>
            </Modal>
            }
            <DragDropRegion disciplineIds={dndData.disciplineIds} lecturerIds={dndData.lecturerIds}
                lecturers={dndData.lecturers} disciplines={dndData.disciplines}/>
    </div>
    )
}

export default App;
