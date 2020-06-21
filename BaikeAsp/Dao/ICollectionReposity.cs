using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface ICollectionReposity
    {
        Task<int> getFavVideoNum(int uid);
        Task<long> isCollect(int uid, int vid);

        void deleteFavVideoByID(int uid, int favid);

        Task<bool> SaveAsync();
    }
}
