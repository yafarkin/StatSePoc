using DataService;
using DataService.Interfaces;
using Prometheus;
using ScriptProviderService;
using ScriptService;
using ScriptService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpMetrics();
app.MapMetrics();

app.MapControllers();

app.Run();