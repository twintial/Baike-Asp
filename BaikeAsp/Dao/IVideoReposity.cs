using BaikeAsp.Dto;
using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IVideoReposity
    {
        Task<List<BKVideoViewModel>> selectVideoByInterVID(int ivid);
    }
}
