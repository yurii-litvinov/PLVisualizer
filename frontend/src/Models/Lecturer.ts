import {Discipline} from "./Discipline";

export interface Lecturer {
    name: string
    position: string
    fullTimePercent: number
    disciplines: Array<Discipline>
    distributedLoad: number
    requiredLoad: number
    isFired: boolean
}