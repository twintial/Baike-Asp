using BaikeAsp.Common;
using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public void changeVideoState(int vid)
        {
            BkInteractiveVideo bkInteractiveVideo = _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(vid))
                                                        .FirstOrDefault();

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
                             InterVideoID = st.InterVideoId,
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
                            bkInteractiveVideo = new BKInteractiveVideoViewModel
                            {
                                interVideoID = st.InterVideoId,
                                videoName = st.VideoName,
                                introduction = st.Introduction,
                                tag = st.Tag,
                                uid = st.UId,
                                playVolume = st.PlayVolume,
                                praisePoint = st.PraisePoint,
                                collectPoint = st.CollectPoint,
                                state = st.State,
                                uploadTime = TimeConvert.ConvertDateTimeToLong(st.UploadTime),
                                icon = st.Icon,
                                initVideoID = st.InitVideoId
                            }
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
                             bkInteractiveVideo = new BKInteractiveVideoViewModel
                             {
                                 interVideoID = st.InterVideoId,
                                 videoName = st.VideoName,
                                 introduction = st.Introduction,
                                 tag = st.Tag,
                                 uid = st.UId,
                                 playVolume = st.PlayVolume,
                                 praisePoint = st.PraisePoint,
                                 collectPoint = st.CollectPoint,
                                 state = st.State,
                                 uploadTime = TimeConvert.ConvertDateTimeToLong(st.UploadTime),
                                 icon = st.Icon,
                                 initVideoID = st.InitVideoId
                             }
                         });

            return await PagedList<BKInterVideoViewModel>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKInterVideoViewModel>> selectByTime(string title, int state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         where st.State.Equals(state) && st.VideoName.Contains(title)
                         orderby TimeConvert.ConvertDateTimeToLong(st.UploadTime) descending
                         select new BKInterVideoViewModel
                         {
                             bkInteractiveVideo = new BKInteractiveVideoViewModel
                             {
                                 interVideoID = st.InterVideoId,
                                 videoName = st.VideoName,
                                 introduction = st.Introduction,
                                 tag = st.Tag,
                                 uid = st.UId,
                                 playVolume = st.PlayVolume,
                                 praisePoint = st.PraisePoint,
                                 collectPoint = st.CollectPoint,
                                 state = st.State,
                                 uploadTime = TimeConvert.ConvertDateTimeToLong(st.UploadTime),
                                 icon = st.Icon,
                                 initVideoID = st.InitVideoId
                             }
                         });

            return await PagedList<BKInterVideoViewModel>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKInteractiveVideoViewModel>> selectFavVideoByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         join gl in _context.BkCollection on st.InterVideoId equals gl.FavVideoId
                         where gl.UId.Equals(uid) && st.State != 0
                         select new BKInteractiveVideoViewModel
                         {
                             interVideoID = st.InterVideoId,
                             videoName = st.VideoName,
                             introduction = st.Introduction,
                             tag = st.Tag,
                             uid = st.UId,
                             playVolume = st.PlayVolume,
                             praisePoint = st.PraisePoint,
                             collectPoint = st.CollectPoint,
                             state = st.State,
                             uploadTime = TimeConvert.ConvertDateTimeToLong(st.UploadTime),
                             icon = st.Icon,
                             initVideoID = st.InitVideoId
                         });

            return await PagedList<BKInteractiveVideoViewModel>.Create(query, pageNum, pageSize);
        }

        public async Task<PagedList<BKInteractiveVideoViewModel>> selectHisVideoByUid(int uid, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         where st.UId.Equals(uid) && st.State.Equals(2)
                         select new BKInteractiveVideoViewModel
                         {
                             interVideoID = st.InterVideoId,
                             videoName = st.VideoName,
                             introduction = st.Introduction,
                             tag = st.Tag,
                             uid = st.UId,
                             playVolume = st.PlayVolume,
                             praisePoint = st.PraisePoint,
                             collectPoint = st.CollectPoint,
                             state = st.State,
                             uploadTime = TimeConvert.ConvertDateTimeToLong(st.UploadTime),
                             icon = st.Icon,
                             initVideoID = st.InitVideoId
                         });

            return await PagedList<BKInteractiveVideoViewModel>.Create(query, pageNum, pageSize);
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

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
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
        public void updateInterVideoStartVideo(int intervideoid, int initvideoid)
        {
            BkInteractiveVideo interactiveVideo = _context.BkInteractiveVideo
                                                    .Where(x => x.InterVideoId.Equals(intervideoid))
                                                    .FirstOrDefault();

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

        public int selectInterVideoStateByID_T(int intervideoid)
        {
            BkInteractiveVideo bkInteractiveVideo = _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(intervideoid))
                                                        .FirstOrDefault();
            return bkInteractiveVideo.State;
        }

        public async Task<int?> selectInitVideoIDByID(int intervideoid)
        {
            BkInteractiveVideo bkInteractiveVideo = await _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(intervideoid))
                                                        .FirstOrDefaultAsync();
            return bkInteractiveVideo.InitVideoId;
        }

        public int? selectInitVideoIDByID_T(int intervideoid)
        {
            BkInteractiveVideo bkInteractiveVideo = _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(intervideoid))
                                                        .FirstOrDefault();
            return bkInteractiveVideo.InitVideoId;
        }

        public async Task<PagedList<BKInteractiveVideoViewModel>> selectInterVideosByUserIf(int uid, VideoState state, int pageNum, int pageSize)
        {
            var query = (from st in _context.BkInteractiveVideo
                         where st.State.Equals(state.GetHashCode()) && st.UId.Equals(uid)
                         select new BKInteractiveVideoViewModel
                         {
                             interVideoID = st.InterVideoId,
                             videoName = st.VideoName,
                             introduction = st.Introduction,
                             tag = st.Tag,
                             uid = st.UId,
                             playVolume = st.PlayVolume,
                             praisePoint = st.PraisePoint,
                             collectPoint = st.CollectPoint,
                             state = st.State,
                             uploadTime = TimeConvert.ConvertDateTimeToLong(st.UploadTime),
                             icon = st.Icon,
                             initVideoID = st.InitVideoId
                         });

            return await PagedList<BKInteractiveVideoViewModel>.Create(query, pageNum, pageSize);
        }

        public async Task<bool> deleteInteractiveVideoByID(int vid)
        {
            BkInteractiveVideo bkInteractiveVideo = await _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(vid))
                                                        .FirstOrDefaultAsync();
            _context.BkInteractiveVideo.Remove(bkInteractiveVideo);
            return true;
        }

        public async Task<BKVideoPlayVideoModel> findVideoPlayPageInfo(int vid)
        {
            BkInteractiveVideo bkInteractiveVideo = await _context.BkInteractiveVideo
                                                        .Where(x => x.InterVideoId.Equals(vid))
                                                        .FirstOrDefaultAsync();

            BkVideo bkVideo = await _context.BkVideo
                .Where(x => x.VideoId.Equals(bkInteractiveVideo.InitVideoId))
                .FirstOrDefaultAsync();

            BKVideoViewModel bKVideoViewModel = new BKVideoViewModel
            {
                videoID = bkVideo.VideoId,
                interVideoID = bkVideo.InterVideoId,
                videoURL = bkVideo.VideoUrl,
                title = bkVideo.Title
            };

            BkUserInfo bkUserInfo = await _context.BkUserInfo
                .Where(x => x.UId.Equals(bkInteractiveVideo.UId))
                .FirstOrDefaultAsync();

            BKUserInfoViewModel bKUserInfoViewModel = new BKUserInfoViewModel
            {
                uid = bkUserInfo.UId,
                nickName = bkUserInfo.NickName,
                iconURL = bkUserInfo.Icon,
                state = bkUserInfo.State,
                introduction = bkUserInfo.Introduction,
                backgroundIconURL = bkUserInfo.BackgroundIcon
            };

            var query = (from st in _context.BkComments
                         join gl in _context.BkUserInfo on st.UId equals gl.UId
                         where st.InterVideoId.Equals(bkInteractiveVideo.InterVideoId)
                         select new CommentViewModel
                         {
                             Uid = gl.UId,
                             InterVideoID = st.InterVideoId,
                             Content = st.Content,
                             SendTime = st.SendTime,
                             icon = gl.Icon,
                             nickName = gl.NickName
                         });

            List<CommentViewModel> list_A = await query.ToListAsync();

            List<BkNextVideo> list_B1 = await _context.BkNextVideo
                .Where(x => x.VideoId.Equals(bkInteractiveVideo.InitVideoId))
                .ToListAsync();

            List<BKNextVideoViewModel> list_B = new List<BKNextVideoViewModel>();

            foreach (BkNextVideo i in list_B1)
            {
                BKNextVideoViewModel p = new BKNextVideoViewModel
                {
                    VideoID = i.VideoId,
                    NextVideoID = i.NextVideoId,
                    Choice = i.Choice
                };
                list_B.Add(p);
            }

            BKVideoPlayVideoModel bKInteractiveVideoViewModel = new BKVideoPlayVideoModel
            {
                interVideoID = bkInteractiveVideo.InterVideoId,
                videoName = bkInteractiveVideo.VideoName,
                introduction = bkInteractiveVideo.Introduction,
                tag = bkInteractiveVideo.Tag,
                uid = bkInteractiveVideo.UId,
                playVolume = bkInteractiveVideo.PlayVolume,
                praisePoint = bkInteractiveVideo.PraisePoint,
                collectPoint = bkInteractiveVideo.CollectPoint,
                state = bkInteractiveVideo.State,
                uploadTime = TimeConvert.ConvertDateTimeToLong(bkInteractiveVideo.UploadTime),
                icon = bkInteractiveVideo.Icon,
                initVideoID = bkInteractiveVideo.InitVideoId,
                initVideo = bKVideoViewModel,
                userInfo = bKUserInfoViewModel,
                comments = list_A,
                nextVideos = list_B
            };
            return bKInteractiveVideoViewModel;
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
