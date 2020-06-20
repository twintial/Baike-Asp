using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkBarrage
    {
        public int UId { get; set; }
        public int VideoId { get; set; }
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
        public double VideoTime { get; set; }
        public string Color { get; set; }
        public int BType { get; set; }

        public virtual BkUser U { get; set; }
        public virtual BkVideo Video { get; set; }
    }
}
