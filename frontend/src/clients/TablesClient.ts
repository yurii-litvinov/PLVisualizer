import axios, {AxiosResponse} from "axios";
import {Discipline} from "../Models/Discipline";
import {Lecturer} from "../Models/Lecturer";

export const baseURL = 'http://localhost:3001/'

export interface ITablesClient {
    exportTable: (id : string, lecturers : Lecturer[]) =>  Promise<AxiosResponse>
    importTable: (id: string) =>  Promise<AxiosResponse<Lecturer[]>>
}

export const createTablesClient = () : ITablesClient => {
    let url = `${baseURL}tables/`
    const exportTable = (id : string, lecturers : Lecturer[]) => axios.post(`${url}id`,lecturers)
    const importTable = (id: string) => axios.get<Lecturer[]>(`${url}id`)
    return {exportTable: exportTable,
        importTable: importTable}
}
