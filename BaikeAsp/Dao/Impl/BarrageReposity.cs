using BaikeAsp.Dto;
using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class BarrageReposity : IBarrageReposity
    {
        private readonly BaikeContext _context;

        public BarrageReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async void insertBarrage(BkBarrage barrage)
        {
            await _context.BkBarrage.AddAsync(barrage);
        }

        public async Task<List<BKBarrage>> selectAllBarragesByID(int vid)
        {
            List<BkBarrage> list = await _context.BkBarrage
                                        .Where(x => x.VideoId.Equals(vid))
                                        .ToListAsync();
            List<BKBarrage> list_ = new List<BKBarrage>();
            foreach (BkBarrage i in list)
            {
                BKBarrage p = new BKBarrage
                {
                    uid = i.UId,
                    videoID = i.VideoId,
                    content = i.Content,
                    sendTime = i.SendTime,
                    videoTime = i.VideoTime,
                    color = i.Color,
                    bType = i.BType
                };
                list_.Add(p);
            }
            return list_;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
