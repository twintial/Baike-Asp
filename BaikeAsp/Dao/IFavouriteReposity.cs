using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IFavouriteReposity
    {
        int GetFollowerCountByUid(int uid);
    }
}
