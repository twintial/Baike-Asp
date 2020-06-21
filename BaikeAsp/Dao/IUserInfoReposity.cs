using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IUserInfoReposity
    {
        Task<IEnumerable<BkUserInfo>> GetBkUserInfo(int uid);
        Task<PagedList<BKUserFollowersDto>> selectUserFollowersByUid(int uid, int pageNum, int pageSize);
        Task<PagedList<BKUserFollowersDto>> selectUsersFollowByUid(int uid, int pageNum, int pageSize);
    }
}
