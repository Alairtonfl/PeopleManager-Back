using PeopleManager.Application.DTOs.Responses;
using PeopleManager.Domain.Exceptions;
using PeopleManager.Domain.Resources;
using System.Net;
using System.Text.Json;

namespace PeopleManager.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
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

            var response = new ApiResponse
            {
                Success = false
            };

            switch (exception)
            {
                case BusinessException be:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = be.Message;
                    break;
                case AuthenticationException ae:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ae.Message;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = BusinessExceptionMsg.EXC0001;
                    break;
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
