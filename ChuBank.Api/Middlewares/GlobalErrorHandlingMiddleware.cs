using System.Net;
using System.Text.Json;
using ChuBank.Domain.Exceptions;

namespace ChuBank.Api.Middlewares;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case ChuBankException e:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; 
                response.Message = e.Message;
                break;

            case KeyNotFoundException e:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound; 
                response.Message = e.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; 
                response.Message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde.";
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}