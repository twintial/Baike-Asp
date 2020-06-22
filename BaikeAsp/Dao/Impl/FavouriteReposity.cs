using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class FavouriteReposity : IFavouriteReposity
    {
        private readonly BaikeContext _context;

        public FavouriteReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async void deleteUsersFollowByID(int uid, int followid)
        {
            BkFavourite bkFavourite = await _context.BkFavourite
                .Where(x => x.UId.Equals(uid) && x.FavUserId.Equals(followid))
                .FirstOrDefaultAsync();
            _context.BkFavourite.Remove(bkFavourite);
        }

        public async Task<int> getUserFollowerNum(int uid)
        {
            return await _context.BkFavourite
                .Where(x => x.FavUserId.Equals(uid))
                .CountAsync();
        }

        public async Task<int> getUsersFollowNum(int uid)
        {
            return await _context.BkFavourite
                .Where(x => x.UId.Equals(uid))
                .CountAsync();
        }

        public void insertUsersFollowByID(int uid, int followid)
        {
            BkFavourite bkFavourite = new BkFavourite
            {
                UId = uid,
                FavUserId = followid
            };
            _context.BkFavourite.AddAsync(bkFavourite);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
