using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkBrowseHistory
    {
        public int UId { get; set; }
        public int WatchVideoId { get; set; }
        public DateTime WatchDate { get; set; }

        public virtual BkUser U { get; set; }
        public virtual BkInteractiveVideo WatchVideo { get; set; }
    }
}
