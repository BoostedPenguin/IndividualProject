using System;
using System.Collections.Generic;

namespace entity_azure_connection.Models
{
    public partial class UserKeywords
    {
        public int KeywordId { get; set; }
        public string UserId { get; set; }
        public string Keyword { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Users User { get; set; }
    }
}
