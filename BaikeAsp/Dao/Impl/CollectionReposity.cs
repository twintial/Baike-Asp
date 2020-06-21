using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class CollectionReposity : ICollectionReposity
    {
        private readonly BaikeContext _context;

        public CollectionReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async void deleteFavVideoByID(int uid, int favid)
        {
            BkCollection bkCollection = await _context.BkCollection
                .Where(x => x.UId.Equals(uid) && x.FavVideoId.Equals(favid)).FirstAsync();
            _context.BkCollection.Remove(bkCollection);
        }

        public async Task<int> getFavVideoNum(int uid)
        {
            return await _context.BkCollection
                .Where(x => x.UId.Equals(uid))
                .CountAsync();
        }

        public async Task<long> isCollect(int uid, int vid)
        {
            return await _context.BkCollection
                .Where(x => x.UId.Equals(uid) && x.FavVideoId.Equals(vid))
                .CountAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
