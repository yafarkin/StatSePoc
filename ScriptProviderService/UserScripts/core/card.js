const sum_avg = require("sum_avg");
const helper = require("./helper");

function handle(data) {
    const tag = data.Tag;
    const userId = data.UserId;

    const calcValues = sum_avg.handle(data);
    
    return {
        card1: {
            text: "Суммарное значение метрик",
            value: calcValues.sum
        },
        card2: {
            text: "Среднее значение метрик",
            value: calcValues.avg
        },
        userId: userId,
        tag: tag,
        text: "Подсчитанные значения"
    };
}

exports.handle = handle;