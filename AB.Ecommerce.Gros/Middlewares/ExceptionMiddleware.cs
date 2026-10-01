using System;
using System.Net;
using System.Text.Json;
using AB.Ecommerce.Gros;
using AB.Ecommerce.Gros.Business;
using AB.Ecommerce.Gros.Shared;
using Serilog;

namespace AB.Ecommerce.Gros
{
	public class ExceptionMiddleware
	{
         
            private readonly RequestDelegate _next;

            public ExceptionMiddleware(RequestDelegate next)
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
                    Log.Error("This is an error message");
                    Log.Error(ex.ToString());
                    Log.Error(ex.StackTrace);

                    await HandleExceptionAsync(context, ex);
                } 
            }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        { 

            var res = new { code = "", message = "Une erreur s'est produite", status = HttpStatusCode.InternalServerError};

            if (exception is ArgumentException)
            {
                res = new { code = "", message = exception.Message, status = HttpStatusCode.BadRequest };
            }

            if (exception is UnauthorizedAccessException)
            {
                res = new { code = "", message = exception.Message, status = HttpStatusCode.Unauthorized };
            }

            if (exception is BusinessException)
            {
                var businessException = (BusinessException)exception;
                res = new { code = businessException.Code, message = exception.Message, status = businessException.Status };
            }
                         
            var result = JsonSerializer.Serialize(res);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)res.status;

            return context.Response.WriteAsync(result);
            }
        }

    
}
