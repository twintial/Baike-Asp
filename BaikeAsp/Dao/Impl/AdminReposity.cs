using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class AdminReposity : IAdminReposity
    {
        private readonly BaikeContext _context;

        public AdminReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task<BkAdmin> Detection(string account, string password)
        {
            return await _context.BkAdmin
                .Where(x => x.Account.Equals(account) && x.Password.Equals(password))
                .FirstAsync();
        }
    }
}
