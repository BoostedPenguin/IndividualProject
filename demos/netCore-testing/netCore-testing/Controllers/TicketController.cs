using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;

namespace netCore_testing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        public ILogger<TicketController> Logger { get; }
        public FirestoreDb db { get; }
        public TicketController(ILogger<TicketController> logger, FirestoreDb db)
        {
            this.Logger = logger;
            this.db = db;
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task PostTodoItem(Ticket ticket)
        {
            var item = db.Collection("tickets").Document();
            Dictionary<string, object> ticketStorage = new Dictionary<string, object>
            {
                { "name", ticket.name },
            };

            await item.SetAsync(ticketStorage);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetItem(string id)
        {
            return await db.Collection("tickets").Document("bcNsldu4RlJUB9AOfy7p").GetSnapshotAsync();
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        //{
        //    return await _context.TodoItems
        //        .Select(x => ItemToDTO(x))
        //        .ToListAsync();
        //}

    }
}
