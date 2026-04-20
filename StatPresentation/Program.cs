using DataService;
using DataService.Interfaces;
using MetricService;
using Prometheus;
using ScriptProviderService;
using ScriptService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMetricsService();

builder.Services.AddDataService();
builder.Services.AddScriptProviderService();
builder.Services.AddScriptService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();
app.UseHttpMetrics();

app.MapMetrics();

app.MapControllers();

app.Run();