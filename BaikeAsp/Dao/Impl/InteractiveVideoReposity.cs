using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
using Microsoft.EntityFrameworkCore;
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
                    InitVideoId = left.InitVideoId,
                    State = left.State,
                    InterVideoId = left.InterVideoId,
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
                    InitVideoId = left.InitVideoId,
                    State = left.State,
                    InterVideoId = left.InterVideoId,
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
                    InitVideoId = left.InitVideoId,
                    State = left.State,
                    InterVideoId = left.InterVideoId,
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
    }
}
