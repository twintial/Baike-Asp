using BaikeAsp.Common;
using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
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
        public void AddInterVideo(BkInteractiveVideo interactiveVideo)
        {
            _context.BkInteractiveVideo.AddAsync(interactiveVideo);
        }

        public void AddVideos(List<BkVideo> videos)
        {
            _context.BkVideo.AddRangeAsync(videos);
        }

        public Task<List<BKNextVideoViewModel>> FindNextVideos(int videoId)
        {
            var query = from nv in _context.BkNextVideo
                        where nv.VideoId == videoId
                        select new BKNextVideoViewModel
                        {
                            Choice = nv.Choice,
                            VideoID = nv.VideoId,
                            NextVideoID = nv.NextVideoId
                        };
            return query.ToListAsync();
        }

        public async Task<int> GetCount(string searchName, string tag)
        {
            var queryExpression = _context.BkInteractiveVideo as IQueryable<BkInteractiveVideo>;
            if (tag != "all" && tag != null)
            {
                queryExpression = queryExpression.Where(x => EF.Functions.Like(x.VideoName, $"%{searchName}%") && x.Tag == tag);
            }
            else
            {
                queryExpression = queryExpression.Where(x => EF.Functions.Like(x.VideoName, $"%{searchName}%"));
            }
            return await queryExpression.Where(x => x.State == 2).CountAsync();
        }

        public Task<string> GetUrlByVId(int videoId)
        {
            var query = from v in _context.BkVideo
                        where v.VideoId == videoId
                        select v.VideoUrl;
            return query.FirstAsync();
        }

        public int GetVideoCountByUid(int uid)
        {
            return _context.BkInteractiveVideo.Where(x => x.UId == uid).Count();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public Task<List<BKSearchInterVideo>> SearchVideos(string searchName, string tag, int page, int size)
        {
            var queryExpression = _context.BkInteractiveVideo as IQueryable<BkInteractiveVideo>;
            if (tag != "all" && tag !=null)
            {
                queryExpression = queryExpression.Where(x => EF.Functions.Like(x.VideoName, $"%{searchName}%") && x.Tag == tag);
            } else
            {
                queryExpression = queryExpression.Where(x => EF.Functions.Like(x.VideoName, $"%{searchName}%"));
            }
            return queryExpression.Join(_context.BkUserInfo, left => left.UId, right => right.UId, (left, right) => new BKSearchInterVideo
                {
                    CollectPoint = left.CollectPoint,
                    Icon = left.Icon,
                    VideoName = left.VideoName,
                    InitVideoID = left.InitVideoId,
                    State = left.State,
                    InterVideoID = left.InterVideoId,
                    Introduction = left.Introduction,
                    PlayVolume = left.PlayVolume,
                    PraisePoint = left.PraisePoint,
                    Tag = left.Tag,
                    Uid = left.UId,
                    UploadTime = TimeConvert.ConvertDateTimeToLong(left.UploadTime),
                    NickName = right.NickName
                })
                .Where(x => x.State == 2)
                .Skip(size * (page - 1))
                .Take(size)
                .ToListAsync();
        }

        public Task<List<BKSearchInterVideo>> SelectByPlayVolume()
        {
            return _context.BkInteractiveVideo.Join(_context.BkUserInfo, left => left.UId, right => right.UId, (left, right) => new BKSearchInterVideo
                {
                    CollectPoint = left.CollectPoint,
                    Icon = left.Icon,
                    VideoName = left.VideoName,
                    InitVideoID = left.InitVideoId,
                    State = left.State,
                    InterVideoID = left.InterVideoId,
                    Introduction = left.Introduction,
                    PlayVolume = left.PlayVolume,
                    PraisePoint = left.PraisePoint,
                    Tag = left.Tag,
                    Uid = left.UId,
                    UploadTime = TimeConvert.ConvertDateTimeToLong(left.UploadTime),
                    NickName = right.NickName
                })
                .Where(x => x.State == 2)
                .OrderByDescending(x => x.PlayVolume)
                .Take(12)
                .ToListAsync();
        }

        public Task<List<BKSearchInterVideo>> SelectByTime()
        {
            return _context.BkInteractiveVideo.Join(_context.BkUserInfo, left => left.UId, right => right.UId, (left, right) => new BKSearchInterVideo
            {
                CollectPoint = left.CollectPoint,
                Icon = left.Icon,
                VideoName = left.VideoName,
                InitVideoID = left.InitVideoId,
                State = left.State,
                InterVideoID = left.InterVideoId,
                Introduction = left.Introduction,
                PlayVolume = left.PlayVolume,
                PraisePoint = left.PraisePoint,
                Tag = left.Tag,
                Uid = left.UId,
                UploadTime = TimeConvert.ConvertDateTimeToLong(left.UploadTime),
                NickName = right.NickName
            })
                .Where(x => x.State == 2)
                .OrderByDescending(x => x.PraisePoint)
                .Take(12)
                .ToListAsync();
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

        public Task<List<BKSearchInterVideo>> SearchVideosByTag(string tag, int page, int size)
        {
            var queryExpression = _context.BkInteractiveVideo as IQueryable<BkInteractiveVideo>;
            queryExpression = queryExpression.Where(x => x.Tag == tag);
            return queryExpression.Join(_context.BkUserInfo, left => left.UId, right => right.UId, (left, right) => new BKSearchInterVideo
            {
                CollectPoint = left.CollectPoint,
                Icon = left.Icon,
                VideoName = left.VideoName,
                InitVideoID = left.InitVideoId,
                State = left.State,
                InterVideoID = left.InterVideoId,
                Introduction = left.Introduction,
                PlayVolume = left.PlayVolume,
                PraisePoint = left.PraisePoint,
                Tag = left.Tag,
                Uid = left.UId,
                UploadTime = TimeConvert.ConvertDateTimeToLong(left.UploadTime),
                NickName = right.NickName
            })
                .Where(x => x.State == 2)
                .Skip(size * (page - 1))
                .Take(size)
                .ToListAsync();
        }
    }
}
