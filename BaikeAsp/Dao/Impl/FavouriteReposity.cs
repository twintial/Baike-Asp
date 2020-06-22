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
        public int GetFollowerCountByUid(int uid)
        {
            return _context.BkFavourite.Where(x => x.FavUserId == uid).Count();
        }
    }
}
