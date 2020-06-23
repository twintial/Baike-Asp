using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKBarrage
    {
        public int uID { get; set; }

        public int videoID { get; set; }

        public string content { get; set; }

        public DateTime sendTime { get; set; }

        public double videoTime { get; set; }

        public string color { get; set; }

        public int bType { get; set; }
    }
}
