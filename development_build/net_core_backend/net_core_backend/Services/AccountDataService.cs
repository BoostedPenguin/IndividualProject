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

        public override Task<Users> Create(Users entity)
        {
            //entity.Auth = httpContext.GetCurrentAuth();
            entity.RoleId = 1;

            return base.Create(entity);
        }

        public async Task<Users> GetAllInformation(int id)
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
