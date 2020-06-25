using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKUserState
    {
        public int uid { get; set; }
        public string nickName { get; set; }
        public string iconURL { get; set; }
        public string introduction { get; set; }
        public int state { get; set; }
    }
}
