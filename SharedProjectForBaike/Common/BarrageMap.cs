using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Common
{
    public class BarrageMap
    {
        public static Hashtable typeMap = new Hashtable();
        public BarrageMap()
        {
            typeMap.Add(0, "right");
            typeMap.Add(1, "top");
            typeMap.Add(2, "bottom");
        }
    }
}
