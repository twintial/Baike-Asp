using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
    }
}
