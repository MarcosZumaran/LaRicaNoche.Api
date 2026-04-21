using System.Net;
using System.Text.Json;
using LaRicaNoche.Api.DTOs.Base;

namespace LaRicaNoche.Api.Middleware;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // BUSCAMOS EL ERROR REAL (InnerException)
            var realMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

            var response = new BaseResponse<string>
            {
                IsSuccess = false,
                Message = "¡Ups! SQL Server dice algo importante.",
                Errors = new List<string> { realMessage }
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
