function handle(data) {
    const tag = data.Tag;
    const userId = data.UserId;
    const userGroupId = data.UserGroupId;
    const metricName = data.MetricName;
    const startDate = data.StartDate;
    const endDate = data.EndDate;
    
    const metricValues = api.MetricApi.GetMetricValues(tag, userId, userGroupId, metricName, startDate, endDate);

    const sum = metricValues.reduce((acc, x) => acc + x.Value, 0);
    const avg = metricValues.length > 0 ? sum / metricValues.length : 0;

    const values = metricValues
        .map(x => x.Value)
        .sort((a, b) => a - b);

    let median = 0;

    if (values.length > 0) {
        const mid = Math.floor(values.length / 2);

        if (values.length % 2 === 0) {
            median = (values[mid - 1] + values[mid]) / 2;
        } else {
            median = values[mid];
        }
    }    
    
    return {
        tag: tag,
        userId: userId,
        sum: sum,
        avg: avg,
        median: median
    };    
}

exports.handle = handle;