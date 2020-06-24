using BaikeAsp.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
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

        public void deleteCollection(BkCollection collection)
        {
            _context.BkCollection.Remove(collection);
        }

        public async Task<bool> deleteFavVideoByID(int uid, int favid)
        {
            BkCollection bkCollection = await _context.BkCollection
                .Where(x => x.UId.Equals(uid) && x.FavVideoId.Equals(favid)).FirstAsync();
            _context.BkCollection.Remove(bkCollection);
            return true;
        }

        public async Task<int> getFavVideoNum(int uid)
        {
            return await _context.BkCollection
                .Where(x => x.UId.Equals(uid))
                .CountAsync();
        }

        public async void insertCollection(BkCollection collection)
        {
            await _context.BkCollection.AddAsync(collection);
        }

        public async void insertFavVideoByID(int uid, int vid)
        {
            BkCollection bkCollection = new BkCollection
            {
                UId = uid,
                FavVideoId = vid
            };
            await _context.BkCollection.AddAsync(bkCollection);
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
        public async Task<int> GetCollectionCountByUid(int uid)
        {
            return await _context.BkCollection.Where(x => x.UId == uid).CountAsync();
        }
    }
}
