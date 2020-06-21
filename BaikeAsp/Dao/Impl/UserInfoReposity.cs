using BaikeAsp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class UserInfoReposity : IUserInfoReposity
    {

        private readonly BaikeContext _context;

        public UserInfoReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task<int> GetState(int uID)
        {
            return await _context.BkUserInfo
                .Where(x => x.UId == uID)
                .Select(x => x.State)
                .FirstAsync();
        }
    }
}
