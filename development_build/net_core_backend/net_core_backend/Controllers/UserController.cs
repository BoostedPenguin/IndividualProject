using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<ExampleController> _logger;
        private readonly IAccountService _context;


        public UserController(ILogger<ExampleController> logger, IAccountService _context)
        {
            this._logger = logger;
            this._context = _context;
        }


        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateAddress(Users entity)
        {
            try
            {
                return Ok(await _context.ChangeAddress(entity));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetUserInfo(int id)
        {
            try
            {
                return Ok(await _context.GetUserInfo(id));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Gets called on post-registration callback
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ValidateUser(Users entity)
        {
            try
            {
                await _context.ValidateUser(entity);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(entity);
        }
    }
}
