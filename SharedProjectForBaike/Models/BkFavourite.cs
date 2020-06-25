using System;
using System.Collections.Generic;

namespace BaikeAsp.Models
{
    public partial class BkFavourite
    {
        public int UId { get; set; }
        public int FavUserId { get; set; }

        public virtual BkUser FavUser { get; set; }
        public virtual BkUser U { get; set; }
    }
}
