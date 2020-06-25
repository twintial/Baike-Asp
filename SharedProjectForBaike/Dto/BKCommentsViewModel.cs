using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKCommentsViewModel
    {
        public int Uid { get; set; }
        public int InterVideoID { get; set; }
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
    }
}
