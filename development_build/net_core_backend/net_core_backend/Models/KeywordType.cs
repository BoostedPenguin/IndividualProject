using System;
using System.Collections.Generic;

namespace net_core_backend.Models
{
    public partial class KeywordType : DefaultModel
    {
        public int KeywordId { get; set; }
        public string Type { get; set; }

        public virtual UserKeywords Keyword { get; set; }
    }
}
