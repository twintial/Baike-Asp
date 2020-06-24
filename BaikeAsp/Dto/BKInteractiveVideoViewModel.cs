using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKInteractiveVideoViewModel
    {
        public int interVideoID { get; set; }

        public string videoName { get; set; }

        public string introduction { get; set; }

        public string tag { get; set; }

        public int uid { get; set; }

        public int playVolume { get; set; }

        public int praisePoint { get; set; }

        public int collectPoint { get; set; }

        public int state { get; set; }

        public DateTime uploadTime { get; set; }

        public string icon { get; set; }

        public int? initVideoID { get; set; }
    }
}
