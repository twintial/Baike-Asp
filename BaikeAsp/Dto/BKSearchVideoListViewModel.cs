using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKSearchVideoListViewModel
    {
        public List<BKInterVideoViewModel> list { get; set; }
        public int pageNum { get; set; }
    }
}
