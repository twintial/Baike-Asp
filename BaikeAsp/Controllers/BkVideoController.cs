using BaikeAsp.Common;
using BaikeAsp.Dao;
using BaikeAsp.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
