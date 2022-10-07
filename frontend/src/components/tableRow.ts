export interface tableRow{
    index: number
    name: string
    post: string
    interestRate: number
    disciplineIds: string[]
    distributedLoad? :{ [key: number] : number}
    standard: number
}