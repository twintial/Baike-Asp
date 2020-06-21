using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Common
{
    public enum ResultCode
    {
        Success = 200,
        Fail = 400
    };
    public class CommonResult
    {
        public static Result Success(object data, string msg)
        {
            return new Result { Code = ResultCode.Success, Data = data, Msg = msg };
        }

        public static Result Success(string msg)
        {
            return new Result { Code = ResultCode.Success, Msg = msg };
        }

        public static Result Fail(object data, string msg)
        {
            return new Result { Code = ResultCode.Fail, Data = data, Msg = msg };
        }

        public static Result Fail(string msg)
        {
            return new Result { Code = ResultCode.Fail, Msg = msg };
        }
    }
}
