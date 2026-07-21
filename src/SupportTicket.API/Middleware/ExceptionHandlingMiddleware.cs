using System.Net;
using System.Text.Json;
using SupportTicket.Application.Exceptions;
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
        var (statusCode, title, detail, errors) = exception switch
        {
            NotFoundException notFound => (HttpStatusCode.NotFound, "Not Found", notFound.Message, null),
            InvalidStatusTransitionException transition => (HttpStatusCode.BadRequest, "Invalid status transition", transition.Message, null),
            AppValidationException validation => (HttpStatusCode.BadRequest, "One or more validation errors occurred.", "One or more validation errors occurred.", validation.Errors),
            ArgumentException argument => (HttpStatusCode.BadRequest, "Bad Request", argument.Message, null),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred", "An internal server error occurred.", null)
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

        var problem = new Dictionary<string, object?>
        {
            ["type"] = $"https://tools.ietf.org/html/rfc7231#section-{(int)statusCode / 100}.{(int)statusCode % 100}",
            ["title"] = title,
            ["status"] = (int)statusCode,
            ["detail"] = detail
        };

        if (errors is not null)
        {
            problem["errors"] = errors;
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
