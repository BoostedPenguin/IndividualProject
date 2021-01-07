using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace net_core_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly IMapper mapper;
        private readonly ITicketService _context;

        public TicketController(ITicketService context, ILogger<TicketController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTicket([FromRoute]int id)
        {
            try
            {
                var result = await _context.GetTicket(id);

                var dto = mapper.Map<SupportTicketViewModel>(result);

                return Ok(dto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUserTickets()
        {
            try
            {
                var result = await _context.GetAllUserTickets();

                var dto = mapper.Map<List<SupportTicketViewModel>>(result);

                return Ok(dto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("admin")]
        [Authorize]
        public async Task<IActionResult> AdminGetAllTickets()
        {
            try
            {
                var result = await _context.AdminGetAllTickets();

                var dto = mapper.Map<SupportTicketViewModel[]>(result);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("message/{ticket_id}")]
        [Authorize]
        public async Task<IActionResult> CreateMessage([FromRoute]int ticket_id, [FromBody]TicketChat chat)
        {
            try
            {
                await _context.CreateMessage(ticket_id, chat);

                var ticket = await _context.GetTicket(ticket_id);

                var dto = mapper.Map<SupportTicketViewModel>(ticket);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTicket([FromBody]SupportTicket ticket)
        {
            try
            {
                var result = await _context.CreateTicket(ticket);

                var dto = mapper.Map<SupportTicketViewModel>(result);

                return Ok(dto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
