import {Guid} from "guid-typescript";

export interface Discipline{
    id: Guid
    content: string
    contactLoad : number
    term: number
    code: string
    educationalProgram: string
    workType: string
}