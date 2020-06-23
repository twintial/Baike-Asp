using BaikeAsp.Dto;
using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao
{
    public interface IBarrageReposity
    {
        Task<List<BKBarrage>> selectAllBarragesByID(int vid);
        void insertBarrage(BkBarrage barrage);
        Task<bool> SaveAsync();
    }
}
