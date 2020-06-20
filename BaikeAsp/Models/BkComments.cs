using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkComments
    {
        public int UId { get; set; }
        public int InterVideoId { get; set; }
        public string Content { get; set; }
        public DateTime SendTime { get; set; }

        public virtual BkInteractiveVideo InterVideo { get; set; }
        public virtual BkUser U { get; set; }
    }
}
