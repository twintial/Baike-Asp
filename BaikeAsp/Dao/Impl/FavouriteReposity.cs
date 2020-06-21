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
    }
}
