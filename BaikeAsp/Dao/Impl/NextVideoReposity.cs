using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class NextVideoReposity : INextVideoReposity
    {
        private readonly BaikeContext _context;

        public NextVideoReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task<bool> deleteNextVideoByID(int vid)
        {
            List<BkNextVideo> bkNextVideo = await _context.BkNextVideo
                                        .Where(x => x.VideoId.Equals(vid))
                                        .ToListAsync();
            for (int i = 0; i < bkNextVideo.Count(); i++)
            {
                _context.BkNextVideo.Remove(bkNextVideo[i]);
            }
            return true;
        }

        public bool deleteNextVideoByID_T(int vid)
        {
            List<BkNextVideo> bkNextVideo = _context.BkNextVideo
                                        .Where(x => x.VideoId.Equals(vid))
                                        .ToList();
            for (int i = 0; i < bkNextVideo.Count(); i++)
            {
                _context.BkNextVideo.Remove(bkNextVideo[i]);
            }
            return true;
        }

        public async Task<bool> insertNextVideo(BkNextVideo bkNextVideo)
        {
            await _context.BkNextVideo.AddAsync(bkNextVideo);
            return true;
        }

        public bool insertNextVideo_T(BkNextVideo bkNextVideo)
        {
            _context.BkNextVideo.Add(bkNextVideo);
            return true;
        }

        public async Task<List<BkNextVideo>> selectNextVideoByVideoID(int vid)
        {
            return await _context.BkNextVideo
                .Where(x => x.VideoId.Equals(vid))
                .ToListAsync();
        }

        public List<BkNextVideo> selectNextVideoByVideoID_T(int vid)
        {
            return _context.BkNextVideo
                .Where(x => x.VideoId.Equals(vid))
                .ToList();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
