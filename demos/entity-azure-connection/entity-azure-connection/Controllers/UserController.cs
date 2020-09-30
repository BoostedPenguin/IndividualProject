using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace entity_azure_connection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserContext _context;

        public UserController(ILogger<UserController> logger, UserContext context)
        {
            _logger = logger;
            _context = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<UserEntity>> GetUsers()
        //{
        //    try
        //    {
        //        using (_context)
        //        {
        //            var result = await _context.Users.ToListAsync();
        //            return Ok(result);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult<UserEntity>> AddUser()
        //{
        //    string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


        //    try
        //    {
        //        using (_context)
        //        {
        //            var user = new UserEntity()
        //            {
        //                Auth_ID = userId,
        //            };
        //            await _context.AddAsync(user);
        //            await _context.SaveChangesAsync();

        //            return Ok(user);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}


        [HttpPost]
        public async Task<ActionResult<UserEntity>> UpdateUser(UserEntity data)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (data == null) return BadRequest();

            try
            {
                using(_context)
                {
                    var user = await _context.FindAsync<UserEntity>(userId);
                    user.City = data.City;
                    await _context.SaveChangesAsync();
                    return Ok(user);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            } 
        }
    }
}
