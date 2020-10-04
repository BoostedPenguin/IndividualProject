using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class UserKeywords : DefaultModel
    {
        public int UserId { get; set; }
        public string Keyword { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Users User { get; set; }
    }
}
