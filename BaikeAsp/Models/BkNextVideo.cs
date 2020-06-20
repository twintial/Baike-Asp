using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkNextVideo
    {
        public int VideoId { get; set; }
        public int NextVideoId { get; set; }
        public string Choice { get; set; }

        public virtual BkVideo NextVideo { get; set; }
        public virtual BkVideo Video { get; set; }
    }
}
