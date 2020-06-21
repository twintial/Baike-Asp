using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class UserReposity : IUserReposity
    {
        private readonly BaikeContext _context;

        public UserReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public int GetUserByAccount(string account)
        {
            return _context.BkUser
                .Where(x => x.Account.Equals(account))
                .Count();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
