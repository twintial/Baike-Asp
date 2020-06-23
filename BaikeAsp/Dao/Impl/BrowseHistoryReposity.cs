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

        public void insertBrowseHistory(BkBrowseHistory bkBrowseHistory)
        {
            _context.BkBrowseHistory.AddAsync(bkBrowseHistory);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void updateBrowseHistory(BkBrowseHistory bkBrowseHistory)
        {
            BkBrowseHistory bkBrowse = _context.BkBrowseHistory
            .Where(x => x.UId.Equals(bkBrowseHistory.UId) && x.WatchVideoId.Equals(bkBrowseHistory.WatchVideoId))
            .FirstOrDefault();

            bkBrowse.WatchDate = bkBrowseHistory.WatchDate;
            _context.Entry(bkBrowse).State = EntityState.Modified;
        }
    }
}
