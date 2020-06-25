using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkUserInfo
    {
        public int UId { get; set; }
        public string NickName { get; set; }
        public string Icon { get; set; }
        public int State { get; set; }
        public string Introduction { get; set; }
        public string BackgroundIcon { get; set; }

        public virtual BkUser U { get; set; }
    }
}
