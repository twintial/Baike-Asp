﻿using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public partial class BKHeadInfoViewModel
    {
        public int uploadVideoNum { get; set; }

        public int favVideoNum { get; set; }

        public int userFollowerNum { get; set; }

        public int usersFollowNum { get; set; }

        public virtual BkUserInfo U { get; set; }

    }
}