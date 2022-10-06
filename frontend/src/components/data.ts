
// этот файл потом удалится, просто для тестов

import {tableRow} from "./TableRow";

export interface IDiscipline{
    index: number
    name: string
}

export const tableData : tableRow[] = [
    {
        name: 'Литвинов Ю.В.',
        post: 'Доцент',
        disciplines : [
            {
                index: 1,
                name: 'Программирование'
            },
            {
                index: 2,
                name: 'Информатика'
            },
        ],
        index: 0,
        interestRate: 100,
        standard: 500
    },

    {
        name: 'Кириленко Я.А.',
        post: 'Старший преподаватель',
        disciplines: [
            {
                index: 3,
                name: 'Windows'
            },
            {
                index: 4,
                name: 'Machine Learning'
            },
        ],
        index: 1,
        interestRate: 1000,
        standard: 500
    },
    {
        name: 'Терехов А.Н.',
        post: 'Старший преподаватель',
        disciplines: [{
            name: 'RuC',
            index: 4
        }],
        index: 2,
        interestRate: 50,
        standard: 500
    },
    {
        name: 'Смирнов К.К.',
        post: 'Старший преподаватель',
        disciplines: [{
            name: 'Асимптотический анализ',
            index: 5
        }],
        index: 3,
        interestRate: 20,
        standard: 500
    },


]
