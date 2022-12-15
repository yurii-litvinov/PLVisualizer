import {FC} from "react";
import {Modal} from "./Modal";
import {List, ListItem  } from "@mui/material";


interface helpModalProps{
    closeModal: () => void
}

export const HelpModal : FC<helpModalProps> = ({closeModal}) =>{
    return(<Modal title={'Помощь'} onSubmit={closeModal} onClose={closeModal}>
        <List>
            <ListItem>
                <a href={"https://docs.google.com/spreadsheets/d/1wQ_8A_4fU2ZplFDtv6neM12CPqVzrOFugjtDqQE6xKc/edit#gid=0"}>
                    Пример входного файла</a>
            </ListItem>
            <ListItem>
                <a href={"https://docs.google.com/spreadsheets/d/1n_5LRJsvkTjXeJAiDXOKJPt4fodr7DsbTN4qK8se6VY/edit#gid=0"}>
                    Пример конфигурационной таблицы</a>
            </ListItem>
        </List>

    </Modal>)

}