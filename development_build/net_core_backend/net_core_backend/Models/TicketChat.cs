using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_core_backend.Models
{
    public partial class TicketChat : DefaultModel
    {
        public int TicketId { get; set; }
        public int CreatorId { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; }

        [NotMapped]
        public bool IsCurrentUser { get; set; } = false;
        public virtual SupportTicket Ticket { get; set; }
    }
}
