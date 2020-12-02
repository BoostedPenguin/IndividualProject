using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_core_backend.Models;
using net_core_backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchDataService context;

        public SearchController(ILogger<SearchController> logger, ISearchDataService context, IMapper mapper)
        {
            _logger = logger;
            this.context = context;
        }

        [HttpGet("{location}")]
        [Authorize]
        public async Task<IActionResult> SearchForLocation(string location)
        {
            try
            {
                var result = await context.SearchForLocation(location);

                //var dto = mapper.Map<List<UserTripsViewModel>>(trips);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("suggestions")]
        [Authorize]
        public async Task<IActionResult> GetSuggestions()
        {
            try
            {
                var result = await context.GetSuggestions();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("suggestions/guest/{latitude}/{longtitude}")]
        public async Task<IActionResult> GetGuestSuggestions(double latitude, double longtitude)
        {
            try
            {
                var data = new UserKeywords() { KeywordAddress = new KeywordAddress() { Latitude = latitude, Longitude = longtitude } };

                var result = await context.GetGuestSuggestions(data);
                
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
