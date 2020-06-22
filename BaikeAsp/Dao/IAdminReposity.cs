using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IAdminReposity
    {
        Task<BkAdmin> Detection(string account, string password);
    }
}
