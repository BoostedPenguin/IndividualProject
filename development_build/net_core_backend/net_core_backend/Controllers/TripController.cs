using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
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
    public class TripController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly ITripService _context;

        public TripController(ITripService context, ILogger<TicketController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserTrips()
        {
            try
            {
                return Ok(await _context.GetUserTrips());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTrip(int id)
        {
            try
            {
                return Ok(await _context.GetTrip(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTrip(int trip_id)
        {
            try
            {
                return Ok(await _context.DeleteTrip(trip_id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("/add/{trip_id}")]
        [Authorize]
        public async Task<IActionResult> AddLocation(int trip_id, UserTripLocations location)
        {
            try
            {
                return Ok(await _context.AddLocation(trip_id, location));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("/remove/{trip_id}")]
        [Authorize]
        public async Task<IActionResult> RemoveLocation(int trip_id, int location_id )
        {
            try
            {
                return Ok(await _context.RemoveLocation(trip_id, location_id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
