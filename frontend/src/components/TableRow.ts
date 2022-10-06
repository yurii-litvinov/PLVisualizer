export interface tableRow{
    index: number
    name: string
    post: string
    interestRate: number
    disciplines: {[key:string] : string}
    disciplinesIds: string[]
    distributedLoad? :{ [key: number] : number}
    standard: number
}