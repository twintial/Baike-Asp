using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> GetCollectionCountByUid(int uid)
        {
            return await _context.BkCollection.Where(x => x.UId == uid).CountAsync();
        }
    }
}
