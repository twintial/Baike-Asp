using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IBrowseHistoryReposity
    {
        Task<bool> deleteBrowseHistoryByID(int uid, int favid);
        void insertBrowseHistory(BkBrowseHistory bkBrowseHistory);
        void updateBrowseHistory(BkBrowseHistory bkBrowseHistory);
        Task<bool> SaveAsync();
    }
}
