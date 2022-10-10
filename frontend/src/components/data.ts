
// этот файл потом удалится, просто для тестов

import {tableRow} from "./tableRow";

// export interface IDiscipline{
//     index: number
//     name: string
// }

export const tableData = {
    disciplines : {
        'Программирование': 'Программирование',
        'Информатика': 'Информатика',
        'Дисциплина с оооооооооооочень очееееень длинннннннннннным названииииииием':
            'Дисциплина с оооооооооооочень очееееень длинннннннннннным названииииииием',
        'Machine Learning' : 'Machine Learning',
        'RuC' : 'RuC',
        'Асимптотический анализ' : 'Асимптотический анализ'},
    lecturers : {
        'Литвинов Ю.В.':
        {
            name: 'Литвинов Ю.В.',
            post: 'Доцент',
            index: 0,
            interestRate: 100,
            standard: 500,
            disciplineIds: ['Программирование', 'Информатика', 'Дисциплина с оооооооооооочень очееееень длинннннннннннным названииииииием']
        },

        'Кириленко Я.А.':
        {
            name: 'Кириленко Я.А.',
            post: 'Старший преподаватель',
            index: 1,
            interestRate: 100,
            standard: 500,
            disciplineIds: []
        },
        'Терехов А.Н.':
        {
            name: 'Терехов А.Н.',
            post: 'Старший преподаватель',
            index: 2,
            interestRate: 50,
            standard: 500,
            disciplineIds: []
        },

        'Смирнов К.К.':
        {
            name: 'Смирнов К.К.',
            post: 'Старший преподаватель',
            index: 3,
            interestRate: 20,
            standard: 500,
            disciplineIds: []
        }
    },

    lecturerIds: ['Литвинов Ю.В.', 'Кириленко Я.А.', 'Терехов А.Н.', 'Смирнов К.К.']
}
