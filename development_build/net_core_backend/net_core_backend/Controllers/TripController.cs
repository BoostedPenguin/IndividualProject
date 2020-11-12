﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.Extensions.Logging;
using net_core_backend.ViewModel;
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
        private readonly IMapper mapper;
        private readonly ITripService _context;

        public TripController(ITripService context, ILogger<TicketController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserTrips()
        {
            try
            {
                var trips = await _context.GetUserTrips();

                var dto = mapper.Map<List<UserTripsViewModel>>(trips);

                return Ok(dto);
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
                var trip = await _context.GetTrip(id);

                var dto = mapper.Map<UserTripsViewModel>(trip);

                return Ok(dto);
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
                var trip = await _context.DeleteTrip(trip_id);

                var dto = mapper.Map<UserTripsViewModel>(trip);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("/add/{trip_id}")]
        [Authorize]
        public async Task<IActionResult> AddLocation(int trip_id, Locations location)
        {
            try
            {
                var result = await _context.AddLocation(trip_id, location);

                var dto = mapper.Map<LocationsViewModel>(result);

                return Ok(dto);
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
                var result = await _context.RemoveLocation(trip_id, location_id);

                var dto = mapper.Map<LocationsViewModel>(result);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}