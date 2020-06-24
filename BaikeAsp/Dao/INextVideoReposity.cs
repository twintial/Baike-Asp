using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface INextVideoReposity
    {
        Task<List<BkNextVideo>> selectNextVideoByVideoID(int vid);
        List<BkNextVideo> selectNextVideoByVideoID_T(int vid);
        Task<bool> deleteNextVideoByID(int vid);
        bool deleteNextVideoByID_T(int vid);
        Task<bool> insertNextVideo(BkNextVideo bkNextVideo);
        bool insertNextVideo_T(BkNextVideo bkNextVideo);
        Task<bool> SaveAsync();
        bool Save();
    }
}
