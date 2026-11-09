using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace OficiosApp.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pasa la solicitud al siguiente middleware o controlador
            await _next(context);
        }
        catch (Exception ex)
        {
            // Serilog registra el error real con toda la traza internamente
            _logger.LogError(ex, "Excepción no controlada capturada por el middleware.");

            await HandleExceptionAsync(context, ex, _env);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment env)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Armamos un JSON estándar ProblemDetails
        var response = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = "Error interno del servidor",
            // Si estamos en desarrollo, mandamos el mensaje del error; en producción, un mensaje genérico seguro.
            Detail = env.IsDevelopment() ? exception.Message : "Ocurrió un error inesperado al procesar la solicitud.",
            Instance = context.Request.Path
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
}