import axios from "axios";
import {Discipline} from "../Models/Discipline";
import {Lecturer} from "../Models/Lecturer";

export const baseURL = 'http://localhost:3001/'

export const createTablesClient = () => {
    let url = `${baseURL}tables/`
    return{
        exportTable: (id : string, lecturers : Lecturer[]) => axios.post(`${url}id`,lecturers),
        importTable: (id: string) => axios.get<Lecturer[]>(`${url}id`)
    }
}