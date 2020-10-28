using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace net_core_backend.Services.Extensions
{
    public static class CurrentExtensions
    {
        /// <summary>
        /// Gets the auth of the issuer
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the current user is an administrator
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        private static bool IsAdmin(this IHttpContextAccessor httpContext, IContextFactory factory)
        {
            using(var a = factory.CreateDbContext())
            {
                var user = a.Users.Where(x => x.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();

                if(user.Result.Role == Role.User)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Determines if the logged in user is either admin or owner of the resource
        /// </summary>
        /// <param name="user"></param>
        /// <param name="httpContext"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static bool HasPrivileges(int userId, IHttpContextAccessor httpContext, IContextFactory factory)
        {
            using (var _context = factory.CreateDbContext())
            {
                var auth = _context.Users.Where(x => x.Id == userId).Select(x => x.Auth).FirstOrDefault();
                
                if (auth == httpContext.GetCurrentAuth() || httpContext.IsAdmin(factory)) return true;

                return false;
            }
        }


        public static async Task<bool> RestrictAdministratorResource(IContextFactory contextFactory, IHttpContextAccessor httpContext)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var creator = await _context.Users.Where(x => x.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();
                if (creator.Role == Role.Admin) return true;
                return false;
            }
        }
    }
}
