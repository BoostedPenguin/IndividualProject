using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class UserKeywordsViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Keyword { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
