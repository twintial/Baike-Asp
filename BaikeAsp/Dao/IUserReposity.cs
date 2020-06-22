using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IUserReposity
    {
        Task<int> CheckUserByAccountAndNickNameAsync(string account, string nickName);
        Task<BkUser> GetUserByAccount(string account);
        void AddUser(BkUser user);
        Task<bool> SaveAsync();
        Task<List<BkUser>> SearchUsers(string nickname, int page, int size);
        Task<List<BkUser>> SearchAllUsers(int page, int size);
        Task<int> GetCount(string nickname);
    }
}
