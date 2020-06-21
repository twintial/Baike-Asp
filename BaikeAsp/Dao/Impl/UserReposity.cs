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

        public void AddUser(BkUser user)
        {
            _context.BkUser.Add(user);
        }

        public async Task<int> CheckUserByAccountAndNickNameAsync(string account, string nickName)
        {
            return await _context.BkUser
                .Where(x => x.Account.Equals(account) || x.BkUserInfo.NickName.Equals(nickName))
                .CountAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
