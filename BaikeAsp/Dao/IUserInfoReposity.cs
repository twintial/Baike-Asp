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
        Task<BKUserInfoViewModel> GetBkUserInfo(int uid);
        Task<PagedList<BKUserFollowersDto>> selectUserFollowersByUid(int uid, int pageNum, int pageSize);
        Task<PagedList<BKUserFollowersDto>> selectUsersFollowByUid(int uid, int pageNum, int pageSize);
        public Task<int> GetState(int uID);
        Task<bool> updateUserInforByID(int uid, string newNickName, string newIntroduction);
        Task<bool> SaveAsync();
        Task<PagedList<BKUserState>> selectByName(string title, int state, int pageNum, int pageSize);
        Task<PagedList<BKUserState>> selectByTime(string title, int state, int pageNum, int pageSize);
        void changeUserState(int uid);
    }
}
