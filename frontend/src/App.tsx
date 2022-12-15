import React, {useState} from 'react';
import './App.css';
import {AppBar} from "./components/Appbar/Appbar";
import {DragDropRegion} from "./components/DragDropRegion/DragDropRegion";
import {Lecturer} from "./Models/Lecturer"
import {Discipline} from "./Models/Discipline";
import {createTablesClient} from "./clients/TablesClient";
import {ExportModal} from "./components/Modal/ExportModal";
import {ImportModal} from "./components/Modal/ImportModal";
import {HelpModal} from "./components/Modal/HelpModal";


function App() {
    const [importModal, setImportModal] = useState(true)
    const [exportModal, setExportModal] = useState(false)
    const [helpModal, setHelpModal] = useState(false)
    const toggleImportModal = () => setImportModal(value => !value)
    const toggleExportModal = () => setExportModal(value => !value)
    const toggleHelpModal = () => setHelpModal(value => !value)

    const [lecturers, setLecturers] = useState<Array<Lecturer>>([])
    const [columnDisciplines, setColumnDisciplines] = useState<Array<Discipline>>([])

    const tablesClient = createTablesClient()

    return(<div className="Appbar">
            <AppBar onImportClick={toggleImportModal} onExportClick={toggleExportModal} onHelpClick={toggleHelpModal}/>
            { importModal && <ImportModal
                setLecturers={setLecturers}
                tablesClient={tablesClient}
                closeModal={toggleImportModal}/>
            }
            { exportModal && <ExportModal
                closeModal={toggleExportModal}
                tablesClient={tablesClient}
                lecturers={lecturers} />
            }
            {helpModal && <HelpModal closeModal={toggleHelpModal}/>}
            <DragDropRegion  lecturers={lecturers} setLecturers={setLecturers} columnDisciplines={columnDisciplines}
                             setColumnDisciplines={setColumnDisciplines} />
    </div>
    )
}

export default App;
