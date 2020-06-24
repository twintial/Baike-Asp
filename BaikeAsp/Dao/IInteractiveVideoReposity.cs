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
        Task<PagedList<BKInteractiveVideoViewModel>> selectFavVideoByUid(int uid, int pageNum, int pageSize);
        Task<PagedList<BKInteractiveVideoViewModel>> selectHisVideoByUid(int uid, int pageNum, int pageSize);
        Task<PagedList<BKInterVidoeInfoWithBrowseHistory>> selectBrowseHistoryByUid(int uid, int pageNum, int pageSize);
        Task<PagedList<BKInterVideoViewModel>> selectByCollectVolume(string title, int state, int pageNum, int pageSize);
        Task<PagedList<BKInterVideoViewModel>> selectByPlayVolume(string title, int state, int pageNum, int pageSize);
        Task<PagedList<BKInterVideoViewModel>> selectByTime(string title, int state, int pageNum, int pageSize);
        Task<PagedList<BKInteractiveVideoViewModel>> selectInterVideosByUserIf(int uid, VideoState state, int pageNum, int pageSize);
        Task<BKVideoPlayVideoModel> findVideoPlayPageInfo(int vid);
        void changeVideoState(int vid);
        void updateInterVideoStartVideo(int intervideoid, int initvideoid);
        Task<bool> deleteInteractiveVideoByID(int vid);
        Task<int> selectInterVideoStateByID(int intervideoid);
        Task<int?> selectInitVideoIDByID(int intervideoid);
        int GetVideoCountByUid(int uid);
        Task<List<BKSearchInterVideo>> SearchVideos(string searchName, string tag, int page, int size);

        Task<List<BKSearchInterVideo>> SearchVideosByTag(string tag, int page, int size);

        Task<int> GetCount(string searchName, string tag);

        Task<List<BKSearchInterVideo>> SelectByPlayVolume();

        Task<List<BKSearchInterVideo>> SelectByTime();

        Task<List<BKNextVideoViewModel>> FindNextVideos(int videoId);

        Task<string> GetUrlByVId(int videoId);

        void AddVideos(List<BkVideo> videos);
        void AddInterVideo(BkInteractiveVideo interactiveVideo);
        Task<bool> SaveAsync();
    }
}
