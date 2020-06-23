using BaikeAsp.Dto;
using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class VideoReposity : IVideoReposity
    {
        private readonly BaikeContext _context;

        public VideoReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<List<BKVideoViewModel>> selectVideoByInterVID(int ivid)
        {
            List<BkVideo> list = await _context.BkVideo
                                .Where(x => x.InterVideo.Equals(ivid))
                                .ToListAsync();
            List<BKVideoViewModel> list_ = new List<BKVideoViewModel>();
            foreach (BkVideo i in list)
            {
                BKVideoViewModel p = new BKVideoViewModel
                {
                    videoID = i.VideoId,
                    interVideoID = i.InterVideoId,
                    videoURL = i.VideoUrl,
                    title = i.Title
                };
                list_.Add(p);
            }
            return list_;
        }
    }
}
