using BrandMicroservice.src.Configs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata;

namespace BrandMicroservice.src.Middlewares.GlobalException
{
    public class GlobalExcepetionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExcepetionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var message = ex.Message;

                if (ex is ArgumentException ar)
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ar.Message;
                }
                else if(ex is UnauthorizedAccessException un)
                {
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = un.Message;
                }
                else if (ex is KeyNotFoundException nf)
                {
                    statusCode = StatusCodes.Status404NotFound;
                    message = nf.Message;
                }

                context.Response.ContentType = Constants.APPLICATION_JSON;
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = message
                });
            }
        }
    }
}
