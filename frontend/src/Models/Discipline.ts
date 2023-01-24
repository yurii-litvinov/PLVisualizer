import {Guid} from "guid-typescript";

export interface LoadDetails {
    loadType: string
    hours: number
    audience: string
}

export interface Discipline {
    id: Guid
    totalLoad : number
    term: string
    code: string
    name: string
    generalWorkType: string
    audience: string
    loadDetails: LoadDetails[]
}