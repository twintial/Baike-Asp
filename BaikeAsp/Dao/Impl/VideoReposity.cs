﻿using BaikeAsp.Models;
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
        public async Task<List<BkVideo>> selectVideoByInterVID(int ivid)
        {
            return await _context.BkVideo
                .Where(x => x.InterVideo.Equals(ivid))
                .ToListAsync();
        }
    }
}