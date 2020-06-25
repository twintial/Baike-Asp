using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKVideoPlayVideoModel : BKInteractiveVideoViewModel
    {
        public BKVideoViewModel initVideo { get; set; }
        public BKUserInfoViewModel userInfo { get; set; }
        public List<CommentViewModel> comments { get; set; }
        public List<BKNextVideoViewModel> nextVideos { get; set; }
    }
}
