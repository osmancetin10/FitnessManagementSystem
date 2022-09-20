using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FitnessManagementSystem.Middlewares
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if(authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodeUsernameAndPassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string usernameAndPassword = encoding.GetString(Convert.FromBase64String(encodeUsernameAndPassword));
                int index = usernameAndPassword.IndexOf(":");
                string username = usernameAndPassword.Substring(0, index);
                string password = usernameAndPassword.Substring(index +1);

                if(username == "admin" && password == "kompanion")
                {
                    await _next.Invoke(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}

