using System.Net;
using Task_Management.Responses;
using FluentValidation;
using Task_Management.Domain.Exceptions;
using System.Text.Json;

namespace Task_Management.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode;
            ApiResponse<object> response;


            switch (exception)
            {
                case ValidationException Validationex:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.Fail("Invalid Input", Validationex.Errors.Select(e => e.ErrorMessage));
                    break;

                case CreateUserException CreateUserex:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.Fail(CreateUserex.Message);
                    break;

                case LoginUserException LoginUserex:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = ApiResponse<object>.Fail(LoginUserex.Message);
                    break;

                case TokenException Tokenex:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = ApiResponse<object>.Fail(Tokenex.Message);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = ApiResponse<object>.Fail("An unexpected error occurred");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}