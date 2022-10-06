import {Discipline} from "./Discipline";

export interface tableRow{
    index: number
    name: string
    post: string
    interestRate: number
    disciplines: Discipline[]
    distributedLoad? :{ [key: number] : number}
    standard: number
}