using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKSearchInterVideo
    {
        public int Uid { get; set; }
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
        public string NickName { get; set; }
    }
}
