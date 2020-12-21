using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.ViewModel;
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
        private readonly IMapper mapper;

        public SearchController(ILogger<SearchController> logger, ISearchDataService context, IMapper mapper)
        {
            _logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{location}")]
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

        [HttpGet("keywords/add/{location}")]
        [Authorize]
        public async Task<IActionResult> AddKeyword([FromRoute]string location)
        {
            try
            {
                var result = await context.AddKeyword(location);

                var dto = mapper.Map<UserKeywordsViewModel[]>(result);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("keywords/clear")]
        [Authorize]
        public async Task<IActionResult> ClearKeywords()
        {
            try
            {
                await context.ClearKeywords();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("keywords/toggle")]
        [Authorize]
        public async Task<IActionResult> ToggleLoggingKeywords()
        {
            try
            {
                await context.ToggleLoggingKeywords();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("keywords")]
        [Authorize]
        public async Task<IActionResult> GetKeywords()
        {
            try
            {
                var result = await context.GetKeywords();

                var dto = mapper.Map<UserKeywordsViewModel[]>(result);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("keywords/logging")]
        [Authorize]
        public async Task<IActionResult> GetLoggingStatus()
        {
            try
            {
                var result = await context.GetLoggingStatus();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("keywords/remove/{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveKeyword([FromRoute]int id)
        {
            try
            {
                var result = await context.RemoveKeyword(id);

                var dto = mapper.Map<UserKeywordsViewModel[]>(result);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("placeid/{placeId}")]
        public async Task<IActionResult> GetPlaceByID([FromRoute]string placeId)
        {
            try
            {
                var result = await context.GetPlaceByID(placeId);

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
        public async Task<IActionResult> GetGuestSuggestions([FromRoute]double latitude, [FromRoute]double longtitude)
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
