function handle(data) {
    log("Input data: " + JSON.stringify(data));

    const sum = data.a + data.b;
    
    const serverTime = api.GetServerTime();

    let result = {
        success: true,
        sum: sum,
        time: serverTime
    };

    // небольшая логика
    if (sum > 100) {
        result.level = "big";
    } else {
        result.level = "small";
    }
    
    result.text = "Hello, World!";
    
    return result;
}