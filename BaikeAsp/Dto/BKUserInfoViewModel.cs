using BaikeAsp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKUserInfoViewModel
    {
        public int uID { get; set; }

        public string nickName { get; set; }

        public string iconURL { get; set; }

        public int state { get; set; }

        public string introduction { get; set; }

        public string backgroundIconURL { get; set; }
    }
}
