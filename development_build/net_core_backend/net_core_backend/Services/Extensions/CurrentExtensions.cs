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
            return httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
