using BaikeAsp.Models;
using BaikeAsp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IInteractiveVideoReposity
    {
        Task<int> getUploadVideoNum(int uid);
        Task<PagedList<BkInteractiveVideo>> selectFavVideoByUid(int uid, int pageNum, int pageSize);
    }
}
