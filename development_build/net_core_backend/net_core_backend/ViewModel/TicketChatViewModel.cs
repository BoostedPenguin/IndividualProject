using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class TicketChatViewModel
    {
        public int TicketId { get; set; }
        public bool IsCurrentUser { get; set; } = false;
        public int CreatorId { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
