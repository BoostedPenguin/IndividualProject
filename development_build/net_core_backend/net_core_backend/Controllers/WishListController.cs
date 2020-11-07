using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WishListController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly IWishListService _context;

        public WishListController(IWishListService context, ILogger<TicketController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                return Ok(await _context.GetWishlist());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> ClearWishlist()
        {
            try
            {
                return Ok(await _context.ClearWishlist());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("/add")]
        [Authorize]
        public async Task<IActionResult> AddLocation(Locations location)
        {
            try
            {
                return Ok(await _context.AddLocation(location));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("/remove/{location_id}")]
        [Authorize]
        public async Task<IActionResult> RemoveLocation(int location_id)
        {
            try
            {
                return Ok(await _context.RemoveLocation(location_id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/create")]
        [Authorize]
        public async Task<IActionResult> CreateTrip()
        {
            try
            {
                return Ok(await _context.CreateTrip());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
