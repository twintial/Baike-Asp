using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKCommentsViewModel
    {
        public int uID { get; set; }
        public int interVideoID { get; set; }
        public string content { get; set; }
        public DateTime sendTime { get; set; }
    }
}
