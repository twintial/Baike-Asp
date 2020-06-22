using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class BrowseHistoryReposity : IBrowseHistoryReposity
    {
        private readonly BaikeContext _context;
        public BrowseHistoryReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async void deleteBrowseHistoryByID(int uid, int favid)
        {
            BkBrowseHistory bkBrowseHistory = await _context.BkBrowseHistory
                .Where(x => x.UId.Equals(uid) && x.WatchVideoId.Equals(favid)).FirstOrDefaultAsync();
            _context.BkBrowseHistory.Remove(bkBrowseHistory);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
