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




// const handleImportSubmit = async () => {
//     const regExp = new RegExp("(?<=^([^/]*/){5})([^/]*)")
//     const matches = regExp.exec(importUrl)
//     const spreadsheetId = matches![0];
//     setLoading(value => !value)
//     if (googleForm){
//         await tablesClient.importTableViaLecturersTableAsync(spreadsheetId)
//             .then(response => {
//             const {data} = response
//             setLecturers(() => data)
//         })
//             .catch(function (error) {
//                 if (error.response) {
//                     const jsonResponse =  JSON.stringify(error.response.data)
//                     const response : Response = JSON.parse(jsonResponse)
//                     setErrorMessage(response.Content)
//                 }})
//     }
//     else {
//         await tablesClient.importTableViaConfigAsync(spreadsheetId, formData).then(response =>{
//             const {data} = response
//             setLecturers(prevState => data)
//             }).catch(function (error){
//             const jsonResponse =  JSON.stringify(error.response.data)
//             const response : Response = JSON.parse(jsonResponse)
//             setErrorMessage(prevState => response.Content)
//             })
//     }

//     setLoading(value => !value)
// }

// return(
//     <Modal onClose={closeModal} title={'Способ импортирования таблицы'} onSubmit={handleImportSubmit}>
//         <SelectImport  xlsxForm={excelForm} setXlsxForm={setExcelForm} setGoogleSSForm={setGoogleSSForm}
//                        formData={formData} setFormData={setFormData}/>
//         <GoogleForm setUrl={setImportUrl} placeholder={excelForm ?
//             'Ссылка на конфигурационную Google Spreadsheet таблицу' :
//         'Ссылка на Google Spreadsheet таблицу с преподавателями'}/>
//         {loading && <LoadingSpinnerContainer>
//             <ColorRing
//                 visible={true}
//                 height="80"
//                 width="80"
//                 margin-left='100px'
//                 ariaLabel="blocks-loading"
//                 wrapperStyle={{}}
//                 wrapperClass="blocks-wrapper"
//                 colors={['#e15b64', '#f47e60', '#f8b26a', '#abbd81', '#849b87']}
//             />
//         </LoadingSpinnerContainer>}
//         {errorMessage !== "" && <h3 style={{marginLeft: "12px"}}>{errorMessage}</h3>}
//     </Modal>)
// }

// const LoadingSpinnerContainer = styled.div` 
// `