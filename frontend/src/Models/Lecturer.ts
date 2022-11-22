import {Discipline} from "./Discipline";

export interface Lecturer{
    name: string
    post: string
    interestRate: number
    disciplines: Discipline[]
    distributedLoad: number
    standard: number
}