import axios, {AxiosResponse} from "axios";
import {Discipline} from "../Models/Discipline";
import {Lecturer} from "../Models/Lecturer";

export const baseURL = 'http://localhost:3001'

export interface ITablesClient {
    exportTableAsync: (id : string, lecturers : Lecturer[]) =>  Promise<AxiosResponse>
    importTableViaLecturersTableAsync: (id: string) =>  Promise<AxiosResponse<Lecturer[]>>
}

export const createTablesClient = () : ITablesClient => {
    let url = `${baseURL}/tables/`
    const exportTableAsync = (id : string, lecturers : Lecturer[]) => axios.post(`${url}/export/id`, lecturers)
    const importTableViaLecturersTableAsync = (id: string) => axios.get<Lecturer[]>(`${url}/import/id`)
    const importLecturersViaConfigAsync = (id: string, FileForm)
    return {exportTableAsync: exportTableAsync,
        importTableViaLecturersTableAsync: importTableViaLecturersTableAsync,
    }
}
