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
        void deleteNextVideoByID(int vid);
        void insertNextVideo(BkNextVideo bkNextVideo);
    }
}
