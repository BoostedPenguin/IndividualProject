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
        private readonly IGoogleService googleService;

        public AccountDataService(IContextFactory contextFactory, IHttpContextAccessor httpContextAccessor, IGoogleService googleService) : base(contextFactory)
        {
            this.contextFactory = contextFactory;
            httpContext = httpContextAccessor;
            this.googleService = googleService;
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
