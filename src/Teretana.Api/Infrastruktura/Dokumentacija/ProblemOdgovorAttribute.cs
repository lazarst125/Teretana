using Microsoft.AspNetCore.Mvc;

namespace Teretana.Api.Infrastruktura.Dokumentacija;

/// <summary>Dokumentuje odgovor greške kao ProblemDetails sa poljem code.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ProblemOdgovorAttribute(int statusCode)
    : ProducesResponseTypeAttribute(typeof(ProblemDetails), statusCode, "application/problem+json");

/// <summary>Dokumentuje 400 odgovor sa greškama po polju.</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ValidacioniProblemOdgovorAttribute()
    : ProducesResponseTypeAttribute(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest, "application/problem+json");
