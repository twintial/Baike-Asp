using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IFavouriteReposity
    {
        Task<int> getUserFollowerNum(int uid);
        Task<int> getUsersFollowNum(int uid);
    }
}
