
// этот файл потом удалится, просто для тестов

import {tableRow} from "./TableRow";

// export interface IDiscipline{
//     index: number
//     name: string
// }

export const tableData = {
    lecturers : {
        'Литвинов Ю.В.':
        {
            name: 'Литвинов Ю.В.',
            post: 'Доцент',
            disciplines : {
                'Программирование': 'Программирование',
                'Информатика' : 'Информатика',
                'Дисциплина с оооооооооооочень очееееень длинннннннннннным названииииииием': 'Дисциплина с оооооооооооочень очееееень длинннннннннннным названииииииием'
            },
            index: 0,
            interestRate: 100,
            standard: 500,
            disciplinesIds: ['Программирование', 'Информатика', 'Дисциплина с оооооооооооочень очееееень длинннннннннннным названииииииием']
        },

        'Кириленко Я.А.':
        {
            name: 'Кириленко Я.А.',
            post: 'Старший преподаватель',
            disciplines : {
                'Machine Learning' : 'Machine Learning'
            },
            index: 1,
            interestRate: 100,
            standard: 500,
            disciplinesIds: ['Machine Learning']
        },
        'Терехов А.Н.':
        {
            name: 'Терехов А.Н.',
            post: 'Старший преподаватель',
            disciplines : {
                'RuC' : 'RuC'
            },
            index: 2,
            interestRate: 50,
            standard: 500,
            disciplinesIds: ['RuC']
        },

        'Смирнов К.К.':
        {
            name: 'Смирнов К.К.',
            post: 'Старший преподаватель',
            disciplines : {
                'Асимптотический анализ': 'Асимптотический анализ'
            },
            index: 3,
            interestRate: 20,
            standard: 500,
            disciplinesIds: ['Асимптотический анализ']
        }
    },

    lecturersIds: ['Литвинов Ю.В.', 'Кириленко Я.А.', 'Терехов А.Н.', 'Смирнов К.К.']
}
