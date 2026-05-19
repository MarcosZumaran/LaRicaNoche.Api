using System.Net;
using System.Text.Json;
using HotelGenericoApi.Models.Exceptions;

namespace HotelGenericoApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessRuleViolationException ex)
        {
            _logger.LogWarning(ex, "Business rule violation: {Message}", ex.Message);
            await WriteProblemDetails(context, HttpStatusCode.Conflict, ex.Message, ex.ErrorCode.ToString());
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation: {Message}", ex.Message);
            await WriteProblemDetails(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteProblemDetails(context, HttpStatusCode.Unauthorized, "No autorizado.");
        }
        catch (KeyNotFoundException ex)
        {
            await WriteProblemDetails(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ExternalServiceException ex)
        {
            _logger.LogError(ex, "External service error: {Message}", ex.Message);
            await WriteProblemDetails(context, HttpStatusCode.BadGateway, ex.Message, ex.ErrorCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteProblemDetails(context, HttpStatusCode.InternalServerError, "Error interno del servidor.");
        }
    }

    private static async Task WriteProblemDetails(HttpContext context, HttpStatusCode statusCode, string detail, string? errorCode = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new Dictionary<string, object?>
        {
            ["type"] = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{(int)statusCode}",
            ["title"] = ((int)statusCode) switch
            {
                400 => "Solicitud inválida",
                401 => "No autorizado",
                404 => "No encontrado",
                409 => "Conflicto",
                502 => "Error de servicio externo",
                _ => "Error interno"
            },
            ["status"] = (int)statusCode,
            ["detail"] = detail,
            ["instance"] = context.Request.Path
        };

        if (errorCode is not null)
            response["errorCode"] = errorCode;

        await context.Response.WriteAsJsonAsync(response);
    }
}
