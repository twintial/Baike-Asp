using BaikeAsp.Dao;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    }
}
