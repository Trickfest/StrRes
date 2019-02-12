using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StrResServices.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace StrResApi.Middleware
{
    public class SecurityMiddleware
    {


        private readonly RequestDelegate _next;

        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ITenantService tenantService)
        {
            Console.WriteLine("*************BEFORE***************");

            await _next(httpContext);

            Console.WriteLine("*************AFTER***************");

            return;
        }
    }

    public static class SecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityMiddleware>();
        }
    }
}