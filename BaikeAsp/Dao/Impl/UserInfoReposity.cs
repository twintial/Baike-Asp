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
        public async Task<IEnumerable<BkUserInfo>> GetBkUserInfo(int uid)
        {
            return await _context.BkUserInfo.Where(x => x.UId.Equals(uid)).ToListAsync();
        }

        public async Task<PagedList<BKUserFollowersDto>> selectUserFollowersByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkUserInfo
                         join gl in _context.BkFavourite on st.UId equals gl.UId
                         where gl.FavUserId.Equals(uid) && st.State != 0
                         select new BKUserFollowersDto
                         {
                             UId = st.UId,
                             NickName = st.NickName,
                             Icon = st.Icon
                         });

            return await PagedList<BKUserFollowersDto>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKUserFollowersDto>> selectUsersFollowByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkUserInfo
                         join gl in _context.BkFavourite on st.UId equals gl.UId
                         where gl.UId.Equals(uid) && st.State != 0
                         select new BKUserFollowersDto
                         {
                             UId = st.UId,
                             NickName = st.NickName,
                             Icon = st.Icon
                         });

            return await PagedList<BKUserFollowersDto>.Create(query, pageNum, pageSize);
        }
    }
}
