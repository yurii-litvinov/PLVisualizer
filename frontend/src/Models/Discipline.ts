import {Guid} from "guid-typescript";

export interface Discipline{
    id: Guid
    content: string
    contactLoad : number
    terms: string
    code: string
    educationalProgram: string
}