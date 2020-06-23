using BaikeAsp.Common;
using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Dao.Impl
{
    public class InteractiveVideoReposity : IInteractiveVideoReposity
    {
        private readonly BaikeContext _context;

        public InteractiveVideoReposity(BaikeContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async void changeVideoState(int vid)
        {
            BkInteractiveVideo bkInteractiveVideo = await _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(vid))
                                                        .FirstOrDefaultAsync();

            bkInteractiveVideo.State = 2 - bkInteractiveVideo.State;

            _context.Entry(bkInteractiveVideo).State = EntityState.Modified;
        }

        public async Task<int> getUploadVideoNum(int uid)
        {
            return await _context.BkInteractiveVideo
                .Where(x => x.UId.Equals(uid) && x.State.Equals(2))
                .CountAsync();
        }

        public async Task<PagedList<BKInterVidoeInfoWithBrowseHistory>> selectBrowseHistoryByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         join gl in _context.BkBrowseHistory on st.InterVideoId equals gl.WatchVideoId
                         where gl.UId.Equals(uid) && st.State != 0
                         orderby gl.WatchDate descending
                         select new BKInterVidoeInfoWithBrowseHistory
                         {
                             InterVideoId = st.InterVideoId,
                             VideoName = st.VideoName,
                             Introduction = st.Introduction,
                             PlayVolume = st.PlayVolume,
                             Icon = st.Icon,
                             WatchDate = gl.WatchDate
                         });

            return await PagedList<BKInterVidoeInfoWithBrowseHistory>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKInterVideoViewModel>> selectByCollectVolume(string title, int state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                        where st.State.Equals(state) && st.VideoName.Contains(title)
                        orderby st.CollectPoint descending
                        select new BKInterVideoViewModel
                        {
                            bkInteractiveVideo = st
                        });

            return await PagedList<BKInterVideoViewModel>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKInterVideoViewModel>> selectByPlayVolume(string title, int state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         where st.State.Equals(state) && st.VideoName.Contains(title)
                         orderby st.PlayVolume descending
                         select new BKInterVideoViewModel
                         {
                             bkInteractiveVideo = st
                         });

            return await PagedList<BKInterVideoViewModel>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKInterVideoViewModel>> selectByTime(string title, int state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         where st.State.Equals(state) && st.VideoName.Contains(title)
                         orderby st.UploadTime descending
                         select new BKInterVideoViewModel
                         {
                             bkInteractiveVideo = st
                         });

            return await PagedList<BKInterVideoViewModel>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BkInteractiveVideo>> selectFavVideoByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         join gl in _context.BkCollection on st.InterVideoId equals gl.FavVideoId
                         where gl.UId.Equals(uid) && st.State != 0
                         select st);

            return await PagedList<BkInteractiveVideo>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BkInteractiveVideo>> selectHisVideoByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         where st.UId.Equals(uid) && st.State.Equals(2)
                         select st);

            return await PagedList<BkInteractiveVideo>.Create(query, pageNum, pageSize);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async void updateInterVideoStartVideo(int intervideoid, int initvideoid)
        {
            BkInteractiveVideo interactiveVideo = await _context.BkInteractiveVideo
                                                    .Where(x => x.InterVideoId.Equals(intervideoid))
                                                    .FirstOrDefaultAsync();

            interactiveVideo.InitVideoId = initvideoid;
            interactiveVideo.State = 2;

            _context.Entry(interactiveVideo).State = EntityState.Modified;
        }

        public async Task<int> selectInterVideoStateByID(int intervideoid)
        {
            BkInteractiveVideo bkInteractiveVideo = await _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(intervideoid))
                                                        .FirstOrDefaultAsync();
            return bkInteractiveVideo.State;
        }

        public async Task<int?> selectInitVideoIDByID(int intervideoid)
        {
            BkInteractiveVideo bkInteractiveVideo = await _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(intervideoid))
                                                        .FirstOrDefaultAsync();
            return bkInteractiveVideo.InitVideoId;
        }

        public async Task<PagedList<BkInteractiveVideo>> selectInterVideosByUserIf(int uid, VideoState state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         where st.State.Equals(state) && st.UId.Equals(uid)
                         select st);

            return await PagedList<BkInteractiveVideo>.Create(query, pageNum, pageSize);
        }

        public async void deleteInteractiveVideoByID(int vid)
        {
            BkInteractiveVideo bkInteractiveVideo = await _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(vid))
                                                        .FirstOrDefaultAsync();
            _context.BkInteractiveVideo.Remove(bkInteractiveVideo);
        }

        public async Task<BkInteractiveVideo> findVideoPlayPageInfo(int vid)
        {
            return await _context.BkInteractiveVideo
                .Where(x => x.InterVideoId.Equals(vid))
                .FirstOrDefaultAsync();
        }
    }
}
