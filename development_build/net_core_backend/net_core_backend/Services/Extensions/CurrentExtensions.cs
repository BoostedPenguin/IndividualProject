using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace net_core_backend.Services.Extensions
{
    public static class CurrentExtensions
    {
        public static string GetCurrentAuth(this IHttpContextAccessor httpContext)
        {
            // Check nameidentifier claim first -> then name claim
            var z = httpContext.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).ToList();
            if(z.Count != 0)
            {
                return httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            
            var b = httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            return b;
        }
    }
}
