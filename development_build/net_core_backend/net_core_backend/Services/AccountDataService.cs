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
        private readonly ContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;


        public AccountDataService(ContextFactory contextFactory, IHttpContextAccessor httpContextAccessor) : base(contextFactory)
        {
            this.contextFactory = contextFactory;
            httpContext = httpContextAccessor;
        }
        
        public async Task<Users> ValidateUser(Users entity)
        {
            if (entity.Auth == null) throw new ArgumentException("Empty user auth");

            using(var _context = contextFactory.CreateDbContext())
            {
                if (await _context.Users.Where(x => x.Auth == entity.Auth).FirstOrDefaultAsync() != null)
                {
                    throw new ArgumentException("User with that auth already exists");
                }

                //Todo - use an intelligent way to assign roles

                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
        }

        public async Task<Users> GetUserInfo(int id)
        {
            using (var _context = contextFactory.CreateDbContext())
            {
                return await _context.Users
                    .Include(x => x.SupportTicket)
                    .Include(x => x.WishList)
                    .Include(x => x.UserKeywords)
                    .Include(x => x.UserTrips)
                    .FirstOrDefaultAsync(x => x.Id == id && x.Auth == httpContext.GetCurrentAuth());
            }
        }

        public async Task<Users> ChangeAddress(Users entity)
        {
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
