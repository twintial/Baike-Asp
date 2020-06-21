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

        void AddUser(BkUser user);

        Task<bool> SaveAsync();
    }
}
