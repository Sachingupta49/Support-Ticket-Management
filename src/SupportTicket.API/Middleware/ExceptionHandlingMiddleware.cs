using System.Net;
using System.Text.Json;
using SupportTicket.Domain.Exceptions;

namespace SupportTicket.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            NotFoundException notFound => (HttpStatusCode.NotFound, "Not Found", notFound.Message),
            InvalidStatusTransitionException transition => (HttpStatusCode.BadRequest, "Invalid status transition", transition.Message),
            ArgumentException argument => (HttpStatusCode.BadRequest, "Bad Request", argument.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred", "An internal server error occurred.")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning(exception, "Handled exception: {Message}", exception.Message);
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            type = $"https://tools.ietf.org/html/rfc7231#section-{(int)statusCode / 100}.{(int)statusCode % 100}",
            title,
            status = (int)statusCode,
            detail
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
