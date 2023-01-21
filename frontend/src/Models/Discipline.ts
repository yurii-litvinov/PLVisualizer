import {Guid} from "guid-typescript";

export interface Discipline {
    id: Guid
    content: string
    load : number
    term: string
    code: string
    name: string
    workType: string
    audience: string
}