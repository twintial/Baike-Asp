using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Common
{
    public class ResourceResult
    {
        public bool Success { get; set; }
        public string error { get; set; }
        public string uuid { get; set; }
        public string type { get; set; }
    }
}
