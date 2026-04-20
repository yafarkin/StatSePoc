const helper = require("helper");
const helper2 = require("helper2");

function handle(data) {
    log("Input data: " + JSON.stringify(data));
    
    const sampleRequest = {
        "text": data.text,
        "number": 123,
        "arr": [1, 2, 3],
        "inner": {
            "guid": "12de8132-b45c-4752-9ef8-b65a2364b4c9"
        }
    };
    
    const sampleResponse = api.SampleApi.CallSample(sampleRequest);

    const sum = data.a + data.b;
    
    const serverTime = api.GetServerTime();

    let result = {
        success: true,
        sum: sum,
        helper_res1: helper.handle(sum),
        helper_res2: helper2.handle(sum),
        time: serverTime,
        sampleResponse: sampleResponse
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

exports.handle = handle;