using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class SupportTicketViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<TicketChatViewModel> TicketChat { get; set; }
    }
}
