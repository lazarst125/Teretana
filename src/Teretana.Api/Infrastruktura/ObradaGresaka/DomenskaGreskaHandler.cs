using Microsoft.AspNetCore.Diagnostics;
using Teretana.Api.Domen;

namespace Teretana.Api.Infrastruktura.ObradaGresaka;

internal sealed partial class DomenskaGreskaHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<DomenskaGreskaHandler> logger) : IExceptionHandler
{
    public const string PoljeKodaGreske = "code";

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DomenskaGreska greska)
        {
            return ValueTask.FromResult(false);
        }

        var status = greska.Vrsta switch
        {
            VrstaGreske.NijeAutorizovan => StatusCodes.Status401Unauthorized,
            VrstaGreske.Zabranjeno => StatusCodes.Status403Forbidden,
            VrstaGreske.NijePronadjeno => StatusCodes.Status404NotFound,
            VrstaGreske.Konflikt => StatusCodes.Status409Conflict,
            VrstaGreske.PoslovnoPravilo => StatusCodes.Status422UnprocessableEntity,
            _ => throw new ArgumentOutOfRangeException(nameof(exception), greska.Vrsta, "Nepoznata vrsta domenske greške."),
        };

        LogDomenskaGreska(logger, greska.Kod, status, httpContext.Request.Method, httpContext.Request.Path.Value);

        httpContext.Response.StatusCode = status;
        return problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Status = status,
                Title = greska.Message,
                Extensions = { [PoljeKodaGreske] = greska.Kod },
            },
        });
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Zahtev {Metod} {Putanja} odbijen: {KodGreske} ({Status})")]
    private static partial void LogDomenskaGreska(ILogger logger, string kodGreske, int status, string metod, string? putanja);
}
