import {Discipline} from "./Discipline";

export interface Lecturer{
    name: string
    post: string
    interestRate: number
    disciplines: Array<Discipline>
    distributedLoad: number
    standard: number
}