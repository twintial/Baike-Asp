using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IUserInfoReposity
    {
        public Task<int> GetState(int uID);
    }
}
