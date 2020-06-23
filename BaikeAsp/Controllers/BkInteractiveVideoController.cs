using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaikeAsp.Models;
using BaikeAsp.Util;
using BaikeAsp.Dto;
using BaikeAsp.Common;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using BaikeAsp.Dao;
using Microsoft.VisualBasic;
using System.Security.Principal;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.IO;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkInteractiveVideoController : ControllerBase
    {
        private readonly IInteractiveVideoReposity interactiveVideoReposity;
        private readonly IBrowseHistoryReposity browseHistoryReposity;
        private readonly ICollectionReposity collectionReposity;
        public BkInteractiveVideoController(IInteractiveVideoReposity interactiveVideo, IBrowseHistoryReposity browseHistory,
            ICollectionReposity collection)
        {
            interactiveVideoReposity = interactiveVideo ?? throw new ArgumentNullException(nameof(interactiveVideoReposity));
            browseHistoryReposity = browseHistory ?? throw new ArgumentNullException(nameof(browseHistoryReposity));
            collectionReposity = collection ?? throw new ArgumentNullException(nameof(ICollectionReposity));
        }

        [HttpGet("video/{state}/{pageNum}")]
        public async Task<ActionResult> getVideoByPage([FromRoute] string state, [FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            try
            {
                VideoState videoState;
                if (state.Equals("publish"))
                {
                    videoState = VideoState.PUBLISH;
                }
                else
                {
                    videoState = VideoState.EDITABLE;
                }
                PagedList<BkInteractiveVideo> result = await interactiveVideoReposity.selectFavVideoByUid(uid, pageNum, 5);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Search Fail"));
            }
        }

        [HttpDelete("video/{interVideoID}")]
        public async Task<ActionResult> deleteInteractiveVideo([FromRoute] int interVideoID)
        {
            try
            {
                if (!DeleteFile.deleteFile(new FileInfo(ResourcePath.coverImgDirURL(interVideoID.ToString()))))
                {
                    return NotFound(CommonResult.Fail("Cover Delete Fail"));
                }
                if (!DeleteFile.deleteFile(new FileInfo(ResourcePath.videoDirURL(interVideoID.ToString()))))
                {
                    return NotFound(CommonResult.Fail("Video Delete Fail"));
                }
                interactiveVideoReposity.deleteInteractiveVideoByID(interVideoID);
                await interactiveVideoReposity.SaveAsync();
                return Ok(CommonResult.Success("Delete Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Delete Fail"));
            }
        }

        [HttpGet("video/{vID}")]
        public async Task<ActionResult> getVideo([FromRoute] int vID)
        {
            try
            {
                BkInteractiveVideo bkInteractiveVideo = await interactiveVideoReposity.findVideoPlayPageInfo(vID);
                return Ok(CommonResult.Success(bkInteractiveVideo, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Search Fail"));
            }
        }

        [HttpPost("history")]
        public async Task<ActionResult> insertHistory([FromBody] BkBrowseHistory bkBrowseHistory)
        {
            bkBrowseHistory.WatchDate = new DateTime();
            try
            {
                browseHistoryReposity.insertBrowseHistory(bkBrowseHistory);
                await browseHistoryReposity.SaveAsync();
                return Ok(CommonResult.Success("Insert Success"));
            }
            catch (Exception)
            {
                browseHistoryReposity.updateBrowseHistory(bkBrowseHistory);
                await browseHistoryReposity.SaveAsync();
                return Ok(CommonResult.Success("Update Success"));
            }
        }

        [HttpPost("insert/collection")]
        public async Task<ActionResult> insertCollection([FromBody] BkCollection collection)
        {
            int? cuid = collection.UId;
            int? fvid = collection.FavVideoId;
            if (cuid == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            else if (fvid == null)
            {
                return NotFound(CommonResult.Fail("Error"));
            }
            try
            {
                collectionReposity.insertCollection(collection);
                await collectionReposity.SaveAsync();
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Already collect this video"));
            }
            return Ok(CommonResult.Success("Insert Success"));
        }

        [HttpDelete("delete/collection/{uID}/{vID}")]
        public ActionResult deleteCollection([FromRoute] int uID, [FromRoute] int vID)
        {
            BkCollection collection = new BkCollection()
            {
                UId = uID,
                FavVideoId = vID
            };
            int? cuid = collection.UId;
            int? fvid = collection.FavVideoId;
            try
            {
                if (cuid == null)
                {
                    return NotFound(CommonResult.Fail("Please Login First"));
                }
                else if (fvid == null)
                {
                    return NotFound(CommonResult.Fail("Error"));
                }
                collectionReposity.deleteCollection(collection);
                return Ok(CommonResult.Success("Delete Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Delete Fail"));
            }
        }
    }
}

