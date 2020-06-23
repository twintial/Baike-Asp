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

        public async void deleteNextVideoByID(int vid)
        {
            List<BkNextVideo> bkNextVideo = await _context.BkNextVideo
                                        .Where(x => x.VideoId.Equals(vid))
                                        .ToListAsync();
            for (int i = 0; i < bkNextVideo.Count(); i++)
            {
                _context.BkNextVideo.Remove(bkNextVideo[i]);
            }
        }

        public async void insertNextVideo(BkNextVideo bkNextVideo)
        {
            await _context.BkNextVideo.AddAsync(bkNextVideo);
        }

        public async Task<List<BkNextVideo>> selectNextVideoByVideoID(int vid)
        {
            return await _context.BkNextVideo
                .Where(x => x.VideoId.Equals(vid))
                .ToListAsync();
        }
    }
}
