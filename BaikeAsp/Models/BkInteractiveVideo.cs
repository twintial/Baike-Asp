using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkInteractiveVideo
    {
        public BkInteractiveVideo()
        {
            BkBrowseHistory = new HashSet<BkBrowseHistory>();
            BkCollection = new HashSet<BkCollection>();
            BkComments = new HashSet<BkComments>();
            BkVideo = new HashSet<BkVideo>();
        }

        public int UId { get; set; }
        public int InterVideoId { get; set; }
        public string VideoName { get; set; }
        public string Introduction { get; set; }
        public int PlayVolume { get; set; }
        public int PraisePoint { get; set; }
        public int CollectPoint { get; set; }
        public string Tag { get; set; }
        public int State { get; set; }
        public DateTime UploadTime { get; set; }
        public string Icon { get; set; }
        public int? InitVideoId { get; set; }

        public virtual BkVideo InitVideo { get; set; }
        public virtual BkUser U { get; set; }
        public virtual ICollection<BkBrowseHistory> BkBrowseHistory { get; set; }
        public virtual ICollection<BkCollection> BkCollection { get; set; }
        public virtual ICollection<BkComments> BkComments { get; set; }
        public virtual ICollection<BkVideo> BkVideo { get; set; }
    }
}
