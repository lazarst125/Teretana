using Microsoft.AspNetCore.Diagnostics;

namespace Teretana.Api.Infrastruktura.ObradaGresaka;

/// <summary>
/// Poslednja linija odbrane: svaki izuzetak koji nijedan sloj nije obradio postaje 500 ProblemDetails
/// bez poruke i stack trace-a, a detalji ostaju samo u logu.
/// </summary>
internal sealed partial class NeobradjenIzuzetakHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<NeobradjenIzuzetakHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        LogNeobradjenIzuzetak(logger, httpContext.Request.Method, httpContext.Request.Path.Value, exception);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Došlo je do neočekivane greške.",
            },
        });
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Neobrađen izuzetak pri obradi zahteva {Metod} {Putanja}")]
    private static partial void LogNeobradjenIzuzetak(ILogger logger, string metod, string? putanja, Exception exception);
}
