using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class CommentReposity : ICommentReposity
    {
        private readonly BaikeContext _context;

        public CommentReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<int> insertComment(BkComments bkComments)
        {
            try
            {
                await _context.BkComments.AddAsync(bkComments);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
