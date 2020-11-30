using AutoMapper;
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
        private readonly IMapper mapper;
        private readonly IAccountService _context;


        public UserController(IAccountService _context, ILogger<ExampleController> logger, IMapper mapper)
        {
            this._logger = logger;
            this.mapper = mapper;
            this._context = _context;
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAddress([FromBody]Users entity)
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
        public async Task<IActionResult> GetUserInfo([FromRoute] int id)
        {
            try
            {
                var result = await _context.GetUserInfo(id);

                var dto = mapper.Map<UsersViewModel>(result);

                return Ok(dto);
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
        public async Task<IActionResult> ValidateUser([FromBody] Users entity)
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
