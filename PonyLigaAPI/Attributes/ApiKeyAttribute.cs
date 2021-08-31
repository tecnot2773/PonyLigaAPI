using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("APIKey", out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided"
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            if (!FindApiKey(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key is not valid"
                };
                return;
            }

            await next();
        }

        public static bool FindApiKey(String extractedApiKey)
        {
            DbContextOptions<PonyLigaAPIContext> options = new DbContextOptions<PonyLigaAPIContext>();
            PonyLigaAPIContext context = new PonyLigaAPIContext(options);
            var apiKeys = context.ApiKeys.ToList();
            var count = context.ApiKeys.Where(s => s.apiKey == extractedApiKey).Count();
            
            if (count >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
