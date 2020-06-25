using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class UserInfoReposity : IUserInfoReposity
    {
        private readonly BaikeContext _context;

        public UserInfoReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<BKUserInfoViewModel> GetBkUserInfo(int uid)
        {
            BkUserInfo bkUserInfo = await _context.BkUserInfo.Where(x => x.UId.Equals(uid)).FirstOrDefaultAsync();
            BKUserInfoViewModel bKUserInfoViewModel = new BKUserInfoViewModel
            {
                uid = bkUserInfo.UId,
                nickName = bkUserInfo.NickName,
                iconURL = bkUserInfo.Icon,
                state = bkUserInfo.State,
                introduction = bkUserInfo.Introduction,
                backgroundIconURL = bkUserInfo.BackgroundIcon
            };
            return bKUserInfoViewModel;
        }

        public async Task<PagedList<BKUserFollowersDto>> selectUserFollowersByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkUserInfo
                         join gl in _context.BkFavourite on st.UId equals gl.UId
                         where gl.FavUserId.Equals(uid) && st.State != 0
                         select new BKUserFollowersDto
                         {
                             uid = st.UId,
                             nickName = st.NickName,
                             iconURL = st.Icon
                         });

            return await PagedList<BKUserFollowersDto>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKUserFollowersDto>> selectUsersFollowByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkUserInfo
                         join gl in _context.BkFavourite on st.UId equals gl.FavUserId
                         where gl.UId.Equals(uid) && st.State != 0
                         select new BKUserFollowersDto
                         {
                             uid = st.UId,
                             nickName = st.NickName,
                             iconURL = st.Icon
                         });

            return await PagedList<BKUserFollowersDto>.Create(query, pageNum, pageSize);
        }

        public async Task<int> GetState(int uID)
        {
            return await _context.BkUserInfo
                .Where(x => x.UId == uID)
                .Select(x => x.State)
                .FirstAsync();
        }

        public async Task<bool> updateUserInforByID(int uid, string newNickName, string newIntroduction)
        {
            BkUserInfo userInfo = await _context.BkUserInfo
                .Where(x => x.UId.Equals(uid))
                .FirstOrDefaultAsync();

            userInfo.NickName = newNickName;
            userInfo.Introduction = newIntroduction;

            _context.Entry(userInfo).State = EntityState.Modified;
            return true;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<PagedList<BKUserState>> selectByName(string title, int state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkUserInfo
                         where st.State.Equals(state) && st.NickName.Contains(title)
                         orderby st.NickName descending
                         select new BKUserState
                         {
                             uid = st.UId,
                             nickName = st.NickName,
                             iconURL = st.Icon,
                             introduction = st.Introduction,
                             state = st.State
                         });

            return await PagedList<BKUserState>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKUserState>> selectByTime(string title, int state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkUserInfo
                         where st.State.Equals(state) && st.NickName.Contains(title)
                         orderby st.UId descending
                         select new BKUserState
                         {
                             uid = st.UId,
                             nickName = st.NickName,
                             iconURL = st.Icon,
                             introduction = st.Introduction,
                             state = st.State
                         });

            return await PagedList<BKUserState>.Create(query, pageNum, pageSize);
        }

        public void changeUserState(int uid)
        {
            BkUserInfo userInfo = _context.BkUserInfo
                .Where(x => x.UId.Equals(uid))
                .FirstOrDefault();

            userInfo.State = 1 - userInfo.State;

            _context.Entry(userInfo).State = EntityState.Modified;
        }

        public async Task<bool> updateUserIconByID(int uid, string icon)
        {
            BkUserInfo userInfo = await _context.BkUserInfo
                .Where(x => x.UId.Equals(uid))
                .FirstOrDefaultAsync();

            userInfo.Icon = icon;

            _context.Entry(userInfo).State = EntityState.Modified;
            return true;
        }

        public async Task<bool> updateUserBackgroundIconByID(int uid, string bgIcon)
        {
            BkUserInfo userInfo = await _context.BkUserInfo
                .Where(x => x.UId.Equals(uid))
                .FirstOrDefaultAsync();

            userInfo.BackgroundIcon = bgIcon;

            _context.Entry(userInfo).State = EntityState.Modified;
            return true;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
