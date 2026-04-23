const sum_avg = require("sum_avg");
const helper = require("./helper");

function handle(data) {
    const tag = data.Tag;
    const userId = data.UserId;

    const calcValues = sum_avg.handle('values', data);
    const calcEvents = sum_avg.handle('events', data);
    
    let storedData = api.MetricApi.GetMetricMetadata(tag, "card key", null, null);
    
    if (!storedData) {
        storedData = { "key": Date.now()};
        api.MetricApi.UpsertMetricMetadata(tag, "card key", null, null, JSON.stringify(storedData), null);
    }
    else
    {
        storedData = JSON.parse(storedData.data);
    }
    
    return {
        card1: {
            text: "Суммарное значение метрик",
            value: calcValues.sum,
            eventValue: calcEvents.sum,
        },
        card2: {
            text: "Среднее значение метрик",
            value: calcValues.avg,
            eventValue: calcEvents.avg,
        },
        userId: userId,
        tag: tag,
        text: "Подсчитанные значения",
        storedData: storedData
    };
}

exports.handle = handle;