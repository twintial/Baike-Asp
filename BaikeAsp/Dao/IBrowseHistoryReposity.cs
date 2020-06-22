using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IBrowseHistoryReposity
    {
        void deleteBrowseHistoryByID(int uid, int favid);
        Task<bool> SaveAsync();
    }
}
