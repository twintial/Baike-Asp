using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Common
{
    public class Result
    {
        public ResultCode Code { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
