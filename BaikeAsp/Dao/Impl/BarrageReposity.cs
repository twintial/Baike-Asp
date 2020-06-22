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

        public async Task<List<BkBarrage>> selectAllBarragesByID(int vid)
        {
            return await _context.BkBarrage
                .Where(x => x.VideoId.Equals(vid))
                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
