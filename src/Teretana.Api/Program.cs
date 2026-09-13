using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Teretana.Api.Infrastruktura.ObradaGresaka;
using Teretana.Api.Infrastruktura.StatusSistema;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole(opcije =>
{
    opcije.IncludeScopes = true;
    opcije.UseUtcTimestamp = true;
    opcije.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<NeobradjenIzuzetakHandler>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = StatusSistemaOdgovor.UpisiAsync,
});

app.Run();
