using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkCollection
    {
        public int UId { get; set; }
        public int FavVideoId { get; set; }

        public virtual BkInteractiveVideo FavVideo { get; set; }
        public virtual BkUser U { get; set; }
    }
}
