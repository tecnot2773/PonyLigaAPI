using System;
using System.Collections.Generic;
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

            if (!findApiKey(extractedApiKey))
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

        public static bool findApiKey(String extractedApiKey)
        {
            DbContextOptions<PonyLigaAPIContext> options = new DbContextOptions<PonyLigaAPIContext>();
            PonyLigaAPIContext context = new PonyLigaAPIContext(options);
            var apiKeys = context.ApiKey.ToList();
            var count = context.ApiKey.Where(s => s.apiKey == extractedApiKey).Count();
            
            if (count >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
