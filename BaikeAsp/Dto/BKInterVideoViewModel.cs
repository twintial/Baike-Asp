using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKInterVideoViewModel
    {
        public string nickName { get; set; }
        public BKInteractiveVideoViewModel bkInteractiveVideo { get; set; }
    }
}
