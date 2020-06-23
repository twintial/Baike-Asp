using BaikeAsp.Common;
using BaikeAsp.Dao;
using BaikeAsp.Dto;
using BaikeAsp.Models;
using BaikeAsp.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkVideoController : ControllerBase
    {
        private readonly IInteractiveVideoReposity _interactiveVideoReposity;

        public BkVideoController(IInteractiveVideoReposity interactiveVideoReposity)
        {
            _interactiveVideoReposity = interactiveVideoReposity;
        }
        [HttpGet("SearchVideo/{title}/{tag}/{page}")]
        public async Task<IActionResult> SearchVideos([FromRoute(Name = "title")] string searchName, string tag, int page)
        {
            var videos = await _interactiveVideoReposity.SearchVideos(searchName, tag, page, 9);
            var count = await _interactiveVideoReposity.GetCount(searchName, tag);
            return Ok(new { List = videos, PageNum = count });
        }

        [HttpGet("SearchVideo/{tag}/{page}")]
        public async Task<IActionResult> SearchAllVideos(string tag, int page)
        {
            var videos = await _interactiveVideoReposity.SearchVideos("", tag, page, 9);
            var count = await _interactiveVideoReposity.GetCount("", tag);
            return Ok(new { List = videos, PageNum = count });
        }

        [HttpGet("category/{tag}/{size}")]
        public async Task<IActionResult> category(string tag, int size)
        {
            var videos = await _interactiveVideoReposity.SearchVideosByTag(tag, 1, size);
            return Ok(videos);
        }

        [HttpGet("playVolume")]
        public async Task<IActionResult> SelectByPlayVolume()
        {
            var videos =  await _interactiveVideoReposity.SelectByPlayVolume();
            return Ok(videos);
        }

        // 有问题，之后解决
        [HttpGet("time")]
        public async Task<IActionResult> SelectByTime()
        {
            var videos = await _interactiveVideoReposity.SelectByTime();
            return Ok(videos);
        }

        [HttpGet("video/next/{videoId}")]
        public async Task<IActionResult> FindNextVideos(int videoId)
        {
            var nextVideos = await _interactiveVideoReposity.FindNextVideos(videoId);
            var url = await _interactiveVideoReposity.GetUrlByVId(videoId);
            return Ok(CommonResult.Success(nextVideos, url));
        }

        [HttpPost("video/upload")]
        public async Task<IActionResult> VideoUpload([FromForm(Name = "file")] IFormFile file)
        {
            var result = await FileUtil.CreateTempFile(file);
            return StatusCode(200, result);
        }

        [HttpPost("cover/upload")]
        public async Task<IActionResult> CoverUpload([FromForm(Name = "file")] IFormFile file)
        {
            var result = await FileUtil.CreateTempFile(file);
            // 用用看这种形式
            return StatusCode(200, result);
        }
        [HttpDelete("upload/{uuid}/{type}")]
        public IActionResult DeleteTemp(string uuid, string type)
        {
            bool success = FileUtil.DeleteTempFile($@"{Path.Combine(ResourcePath.TEMP, $@"{uuid}.{type}")}");
            return StatusCode(200, success);
        }
        [HttpPost("video/submit")]
        public async Task<IActionResult> Submit([FromBody] BKVideoUploadViewModel uploadViewModel)
        {
            // 判断是否登陆
            int? uId = HttpContext.Session.GetInt32("userID");
            if (uId == null)
            {
                return Ok(CommonResult.Fail("Please Login"));
            }
            // 插入互动视频
            var interVideoModel = new BkInteractiveVideo
            {
                UId = (int)uId,
                State = 1,
                UploadTime = new DateTime(),
                Icon = uploadViewModel.Icon,
                VideoName = uploadViewModel.VideoName,
                Introduction = uploadViewModel.Introduction,
                Tag = uploadViewModel.Tag
            };
            _interactiveVideoReposity.AddInterVideo(interVideoModel);
            // 移动封面
            string coverPath = Path.Combine(ResourcePath.VIDEO_COVER, interVideoModel.InterVideoId.ToString());
            Directory.CreateDirectory(coverPath);
            System.IO.File.Move(Path.Combine(ResourcePath.TEMP, uploadViewModel.CoverUUID),
                Path.Combine(coverPath, uploadViewModel.CoverUUID));
            // 移动视频
            string videoPath = Path.Combine(ResourcePath.VIDEO, uploadViewModel.InterVideoID.ToString());
            foreach(var videoUUID in uploadViewModel.VideoFilesUUID)
            {
                System.IO.File.Move(Path.Combine(ResourcePath.TEMP, videoUUID),
                Path.Combine(videoPath, videoUUID));
            }

            // 插入视频
            List<BkVideo> videos = new List<BkVideo>();
            for(int i = 0; i < uploadViewModel.VideoFilesUUID.Count; i++)
            {
                var video = new BkVideo
                {
                    InterVideoId = uploadViewModel.InterVideoID,
                    Title = $@"p{i + 1}_{uploadViewModel.VIdeoNames[i]}",
                    VideoUrl = Path.Combine("video", uploadViewModel.InterVideoID.ToString(), uploadViewModel.VideoFilesUUID[i])
                };
                videos.Add(video);
            }
            _interactiveVideoReposity.AddVideos(videos);
            await _interactiveVideoReposity.SaveAsync();
            return Ok();
        }
    }
}
