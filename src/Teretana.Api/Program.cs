using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Teretana.Api.Infrastruktura.ObradaGresaka;
using Teretana.Api.Infrastruktura.StatusSistema;
using Teretana.Api.Podaci;

const string ArgumentZaResetBaze = "--reset-db";

var builder = WebApplication.CreateBuilder([.. args.Where(argument => argument != ArgumentZaResetBaze)]);

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole(opcije =>
{
    opcije.IncludeScopes = true;
    opcije.UseUtcTimestamp = true;
    opcije.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
});

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<NeobradjenIzuzetakHandler>();
builder.Services.DodajBazuPodataka();
builder.Services.AddHealthChecks().AddDbContextCheck<TeretanaDbContext>("baza");

var app = builder.Build();

if (args.Contains(ArgumentZaResetBaze))
{
    Environment.ExitCode = await PripremaBaze.ResetujIzKomandneLinijeAsync(app);
    return;
}

await PripremaBaze.PripremiAsync(app.Services);

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = StatusSistemaOdgovor.UpisiAsync,
});

app.Run();
