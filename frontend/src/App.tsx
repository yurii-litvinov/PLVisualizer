import React, {useState, useEffect} from 'react';
import './App.css';
import {AppBar} from "./components/Appbar/Appbar";
import {DragDropRegion} from "./components/DragDropRegion/DragDropRegion";
import {Lecturer} from "./Models/Lecturer"
import {Discipline} from "./Models/Discipline";
import {createTablesClient} from "./clients/TablesClient";
import {ExportModal} from "./components/Modal/ExportModal";
import {ImportModal} from "./components/Modal/ImportModal";
import {HelpModal} from "./components/Modal/HelpModal";

enum LoadingState {
    Idle,
    InProgress,
    Done
}

function App() {
    const spreadsheetId = "14KH_E58b4IuuZO40jQZoUZSVUsbszP_sI_wteubjRs0"

    const [importModal, setImportModal] = useState(false)
    const [exportModal, setExportModal] = useState(false)
    const [helpModal, setHelpModal] = useState(false)
    const [loadingState, setLoadingState] = useState(LoadingState.Idle)
    const toggleImportModal = () => setImportModal(value => !value)
    const toggleExportModal = () => setExportModal(value => !value)
    const toggleHelpModal = () => setHelpModal(value => !value)

    const [lecturers, setLecturers] = useState<Array<Lecturer>>([])
    const [columnDisciplines, setColumnDisciplines] = useState<Array<Discipline>>([])

    const tablesClient = createTablesClient()

    useEffect(() => {
        if (loadingState === LoadingState.Idle) {
            setLoadingState(LoadingState.InProgress)
            const load = async () => {
                await tablesClient.importTableViaConfigAsync(spreadsheetId).then(response => {
                    const {data} = response
                    setLecturers(prevState => data)
                }).catch(function (error) {
                    // const jsonResponse =  JSON.stringify(error.response.data)
                    // const response : Response = JSON.parse(jsonResponse)
                    // setErrorMessage(prevState => response.Content)
                })
                setLoadingState(LoadingState.Done)
            }
            load()
        }
    }, [loadingState, tablesClient])

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