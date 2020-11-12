using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using net_core_backend.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class AccountDataService : DataService<Users>, IAccountService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;


        public AccountDataService(IContextFactory contextFactory, IHttpContextAccessor httpContextAccessor) : base(contextFactory)
        {
            this.contextFactory = contextFactory;
            httpContext = httpContextAccessor;
        }
        
        /// <summary>
        /// Handles post-registration callback
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Users> ValidateUser(Users entity)
        {
            if (entity.Auth == null) throw new ArgumentException("Empty user auth");

            using(var _context = contextFactory.CreateDbContext())
            {
                if (await _context.Users.Where(x => x.Auth == entity.Auth).FirstOrDefaultAsync() != null)
                {
                    throw new ArgumentException("User with that auth already exists");
                }

                await _context.AddAsync(entity);
                await _context.AddAsync(new WishList() { UserId = entity.Id });
                await _context.SaveChangesAsync();

                return entity;
            }
        }

        //public async Task AddKeyword(string keyword)
        //{
        //    if (keyword == null) throw new ArgumentException("Keyword was empty");

        //    var keywords = keyword.Split(null);

        //    List<string> keywordsLog = new List<string>();

        //    keywordsLog.AddRange(keywords.Where(x => x.Length < 15).ToArray());

        //    using(var a = contextFactory.CreateDbContext())
        //    {
        //        var currentKeywords = await a.UserKeywords.Include(x => x.User).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).ToListAsync();

        //        var userId = await GetUserId(httpContext.GetCurrentAuth());

        //        var newKeywords = keywordsLog.Where(x => currentKeywords.All(y => y.Keyword.ToLower() != x.ToLower()));

        //        List<UserKeywords> toBeAdded = new List<UserKeywords>();

        //        foreach(var b in newKeywords)
        //        {
        //            toBeAdded.Add(new UserKeywords() { Keyword = b, UserId = userId });
        //        }

        //        await a.UserKeywords.AddRangeAsync(toBeAdded);
        //        await a.SaveChangesAsync();
        //    }
        //}

        public async Task AddKeyword(string keyword)
        {
            if (keyword == null) throw new ArgumentException("Keyword was empty");

            if (keyword.Length > 20) throw new ArgumentException("Keyword is too large");

            using (var a = contextFactory.CreateDbContext())
            {
                var currentKeywords = await a.UserKeywords.Include(x => x.User).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).ToListAsync();

                var userId = await GetUserId(httpContext.GetCurrentAuth());

                if (currentKeywords.Any(x => x.Keyword.ToLower() == keyword.ToLower())) throw new ArgumentException("Keywords already in system");

                await a.AddAsync(new UserKeywords() { Keyword = keyword, UserId = userId });
                await a.SaveChangesAsync();
            }
        }

        public async Task ClearKeywords()
        {
            using(var a = contextFactory.CreateDbContext())
            {
                var currentKeywords = await a.UserKeywords.Include(x => x.User).Where(x => x.User.Auth == httpContext.GetCurrentAuth()).ToListAsync();

                a.RemoveRange(currentKeywords);

                await a.SaveChangesAsync();
            }
        }

        public async Task<Users> GetUserInfo(int id)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                var user = await _context.Users
                    .Include(x => x.SupportTicket)
                    .Include(x => x.WishList)
                    .Include(x => x.UserKeywords)
                    .Include(x => x.UserTrips)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (CurrentExtensions.HasPrivileges(user.Id, httpContext, contextFactory)) return user;

                throw new ArgumentException("Access forbidden!");
            }
        }

        public async Task<Users> ChangeAddress(Users entity)
        {
            if (await CurrentExtensions.RestrictAdministratorResource(contextFactory, httpContext))
            {
                throw new ArgumentException("Administrators cannot change their address!");
            }

            using (var _context = contextFactory.CreateDbContext())
            {
                var result = await _context.Users.Where(x => x.Auth == httpContext.GetCurrentAuth()).FirstOrDefaultAsync();
                result.City = entity.City;
                result.Country = entity.Country;

                await _context.SaveChangesAsync();

                return result;
            }
        }
    }
}
