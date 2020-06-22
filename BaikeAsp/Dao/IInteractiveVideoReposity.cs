using BaikeAsp.Dto;
using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IInteractiveVideoReposity
    {
        int GetVideoCountByUid(int uid);
        Task<List<BKSearchInterVideo>> SearchVideos(string searchName, string tag, int page, int size);

        Task<int> GetCount(string searchName, string tag);
    }
}
