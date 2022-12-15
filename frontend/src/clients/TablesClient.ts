import axios, {AxiosResponse} from "axios";
import {Lecturer} from "../Models/Lecturer";


export interface ITablesClient {
    exportTableAsync: (spreadsheetId : string, lecturers : Lecturer[]) =>  Promise<AxiosResponse>
    importTableViaLecturersTableAsync: (spreadsheetId: string) =>  Promise<AxiosResponse<Lecturer[]>>
    importTableViaConfigAsync: (spreadsheetId: string, file : FormData) => Promise<AxiosResponse<Lecturer[]>>
    
}

export const createTablesClient = () : ITablesClient => {
    const url = 'https://localhost:8787/tables'
    const exportTableAsync = (spreadsheetId : string, lecturers : Lecturer[]) => axios.post(`${url}/export/${spreadsheetId}`, lecturers)
    const importTableViaLecturersTableAsync = (spreadsheetId: string) => axios.get<Lecturer[]>(`${url}/import/${spreadsheetId}`)
    const importTableViaConfigAsync = (spreadsheetId: string, file: FormData) => axios.post<Lecturer[]>(`${url}/import/config/${spreadsheetId}`, file)
    return {exportTableAsync: exportTableAsync,
        importTableViaLecturersTableAsync: importTableViaLecturersTableAsync,
        importTableViaConfigAsync: importTableViaConfigAsync
    }
}
