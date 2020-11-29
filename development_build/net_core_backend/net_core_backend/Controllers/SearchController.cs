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
    }
}
