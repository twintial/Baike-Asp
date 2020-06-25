using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkVideo
    {
        public BkVideo()
        {
            BkBarrage = new HashSet<BkBarrage>();
            BkInteractiveVideo = new HashSet<BkInteractiveVideo>();
            BkNextVideoNextVideo = new HashSet<BkNextVideo>();
            BkNextVideoVideo = new HashSet<BkNextVideo>();
        }

        public int VideoId { get; set; }
        public int InterVideoId { get; set; }
        public string VideoUrl { get; set; }
        public string Title { get; set; }

        public virtual BkInteractiveVideo InterVideo { get; set; }
        public virtual ICollection<BkBarrage> BkBarrage { get; set; }
        public virtual ICollection<BkInteractiveVideo> BkInteractiveVideo { get; set; }
        public virtual ICollection<BkNextVideo> BkNextVideoNextVideo { get; set; }
        public virtual ICollection<BkNextVideo> BkNextVideoVideo { get; set; }
    }
}
