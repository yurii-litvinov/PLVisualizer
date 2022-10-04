import React, {FC, useState} from 'react';
import './App.css';
import {DragDropContext} from 'react-beautiful-dnd'
import {AppBar} from "./components/Appbar/Appbar";
import {Modal} from "./components/Modal/Modal";
import {SelectImport} from "./components/Modal/SelectImport";
import {lecturers} from "./components/data";
import {Column} from "./components/Column";
import styled from "styled-components"

const LecturersColumnContainer = styled.div`
margin-right: 80px;
  height: 600px;
  width: 30%;
`

function App() {
    const onDragEnd = () => {

    }

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
            <DragDropContext onDragEnd={onDragEnd}>
                <LecturersColumnContainer>
                    <Column  title={'Преподаватели'} children={lecturers} id={'column-1'} />
                </LecturersColumnContainer>
            </DragDropContext>
    </div>
    )
}

export default App;
