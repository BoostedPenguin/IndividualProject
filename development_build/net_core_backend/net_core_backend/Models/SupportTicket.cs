using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class SupportTicket
    {
        public SupportTicket()
        {
            TicketChat = new HashSet<TicketChat>();
        }

        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<TicketChat> TicketChat { get; set; }
    }
}
