using System;
using System.Collections.Generic;

namespace entity_azure_connection.Models
{
    public partial class TicketChat
    {
        public int ChatId { get; set; }
        public int TicketId { get; set; }
        public string CreatorId { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual SupportTicket Ticket { get; set; }
    }
}
