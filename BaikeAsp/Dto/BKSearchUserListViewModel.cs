using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dto
{
    public class BKSearchUserListViewModel
    {
        public List<BKSearchUser> List { get; set; }
        public int PageNum { get; set; }
        public List<int> Follow { get; set; }
        public List<int> Video { get; set; }
    }
}
