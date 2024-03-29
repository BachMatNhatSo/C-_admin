using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebsiteAdmin.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SecretKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;
        public SecretKeyMiddleware(RequestDelegate next, string secretKey)
        {
            _next = next;
            _secretKey = secretKey;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Your middleware logic here
            var requestSecretKey = context.Request.Headers["Secret-Key"].ToString();

            if (requestSecretKey != _secretKey)
            {
                context.Response.StatusCode = 401; // Unauthorized
                context.Response.ContentType = "application/json";
                var jsonResponse = JsonConvert.SerializeObject(new { message = "Unauthorized: Invalid secret key" });
                await context.Response.WriteAsync(jsonResponse);
                return;
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SecretKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecretKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecretKeyMiddleware>("secretKey");
        }
    }
}
