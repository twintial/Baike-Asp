using BaikeAsp.Common;
using BaikeAsp.Dto;
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
        Task<PagedList<BkInteractiveVideo>> selectHisVideoByUid(int uid, int pageNum, int pageSize);
        Task<PagedList<BKInterVidoeInfoWithBrowseHistory>> selectBrowseHistoryByUid(int uid, int pageNum, int pageSize);
        Task<PagedList<BKInterVideoViewModel>> selectByCollectVolume(string title, int state, int pageNum, int pageSize);
        Task<PagedList<BKInterVideoViewModel>> selectByPlayVolume(string title, int state, int pageNum, int pageSize);
        Task<PagedList<BKInterVideoViewModel>> selectByTime(string title, int state, int pageNum, int pageSize);
        Task<PagedList<BkInteractiveVideo>> selectInterVideosByUserIf(int uid, VideoState state, int pageNum, int pageSize);
        Task<BkInteractiveVideo> findVideoPlayPageInfo(int vid);
        void changeVideoState(int vid);
        void updateInterVideoStartVideo(int intervideoid, int initvideoid);
        void deleteInteractiveVideoByID(int vid);
        Task<int> selectInterVideoStateByID(int intervideoid);
        Task<int?> selectInitVideoIDByID(int intervideoid);
        Task<bool> SaveAsync();
    }
}
