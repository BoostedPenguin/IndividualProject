using entity_azure_connection.Models;
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
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly IndividualProjectContext _context;


        public MainController(ILogger<MainController> logger, IndividualProjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<SupportTicket>> GetTickets()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                var items = await _context.SupportTicket.Include(x => x.TicketChat).ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            //var tickets = await _context.SupportTicket.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
