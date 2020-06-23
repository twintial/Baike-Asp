using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface ICommentReposity
    {
        Task<int> insertComment(BkComments bkComments);
    }
}
