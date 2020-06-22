using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public interface ICollectionReposity
    {
        Task<int> GetCollectionCountByUid(int uid);
    }
}
