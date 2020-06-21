using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IUserReposity
    {
        int GetUserByAccount(string account);

        Task<bool> SaveAsync();
    }
}
