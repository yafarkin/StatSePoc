const base = require_base("hello");

function handle(data) {
    const result = base.handle(data);

    result.description = "Handled by custom code";

    return result;
}
