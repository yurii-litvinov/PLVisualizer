import axios, {AxiosResponse} from "axios";
import {Discipline} from "../Models/Discipline";
import {Lecturer} from "../Models/Lecturer";


export interface ITablesClient {
    exportTableAsync: (spreadsheetId : string, lecturers : Lecturer[]) =>  Promise<AxiosResponse>
    importTableViaLecturersTableAsync: (spreadsheetId: string) =>  Promise<AxiosResponse<Lecturer[]>>
    importTableViaConfigAsync: (spreadsheetId: string, file : FormData) => Promise<AxiosResponse<Lecturer[]>>
    
}

export const createTablesClient = () : ITablesClient => {
    let url = 'https://localhost:5001/tables'
    const exportTableAsync = (spreadsheetId : string, lecturers : Lecturer[]) => axios.post(`${url}/export/${spreadsheetId}`, lecturers)
    const importTableViaLecturersTableAsync = (spreadsheetId: string) => axios.get<Lecturer[]>(`${url}/import/${spreadsheetId}`)
    const importTableViaConfigAsync = (spreadsheetId: string, file: FormData) => axios.post<Lecturer[]>(`${url}/import/config/${spreadsheetId}`)
    return {exportTableAsync: exportTableAsync,
        importTableViaLecturersTableAsync: importTableViaLecturersTableAsync,
        importTableViaConfigAsync: importTableViaConfigAsync
    }
}
