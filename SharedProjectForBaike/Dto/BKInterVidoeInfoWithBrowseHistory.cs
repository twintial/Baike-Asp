using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKInterVidoeInfoWithBrowseHistory
    {
        public int InterVideoID { get; set; }
        public string VideoName { get; set; }
        public string Introduction { get; set; }
        public int PlayVolume { get; set; }
        public string Icon { get; set; }
        public DateTime WatchDate { get; set; }
    }
}
