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

        public async Task<int> GetCount(string nickname)
        {
            var queryExpression = _context.BkUser as IQueryable<BkUser>;
            if (nickname != null)
            {
                queryExpression = queryExpression
                    .Where(x => EF.Functions.Like(x.BkUserInfo.NickName, $"%{nickname}%"));
            }
            return await queryExpression.CountAsync();
        }

        public async Task<BkUser> GetUserByAccount(string account)
        {
            return await _context.BkUser
                .Where(x => x.Account.Equals(account))
                .FirstAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<List<BkUser>> SearchAllUsers(int page, int size)
        {
            return await _context.BkUser
                 .Include(x => x.BkUserInfo)
                 .Skip(size * (page - 1))
                 .Take(size)
                 .ToListAsync();
        }

        public async Task<List<BkUser>> SearchUsers(string nickname, int page, int size)
        {
            return await _context.BkUser
                .Where(x => EF.Functions.Like(x.BkUserInfo.NickName, $"%{nickname}%"))
                .Include(x => x.BkUserInfo)
                .Skip(size * (page - 1))
                .Take(size)
                .ToListAsync();
        }
    }
}
