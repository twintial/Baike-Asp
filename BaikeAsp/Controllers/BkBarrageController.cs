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

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkBarrageController : ControllerBase
    {
        private readonly IBarrageReposity barrageReposity;
        public BkBarrageController(IBarrageReposity barrage)
        {
            barrageReposity = barrage ?? throw new ArgumentNullException(nameof(barrageReposity));
        }

        [HttpPost("danmaku/v2")]
        public async Task<ActionResult> Send([FromBody] BKBarrageViewModel bKBarrageViewModel)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            try
            {
                int nRGB = RGBColorToIntUtil.RGBToInt(bKBarrageViewModel.color);
                BkBarrage barrage = new BkBarrage
                {
                    UId = int.Parse(bKBarrageViewModel.author),
                    Content = bKBarrageViewModel.text,
                    VideoTime = bKBarrageViewModel.time,
                    SendTime = DateTime.Now,
                    Color = bKBarrageViewModel.color,
                    BType = (int)BarrageMap.typeMap[bKBarrageViewModel.type],
                    VideoId = bKBarrageViewModel.Player
                };
                barrageReposity.insertBarrage(barrage);
                await barrageReposity.SaveAsync();
                return Ok(CommonResult.Success("Send Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Send Fail"));
            }
        }

        [HttpGet("danmaku/v2/{vID}")]
        public async Task<ActionResult> Read([FromRoute] int vid)
        {
            try
            {
                List<BKBarrage> proBarrages = await barrageReposity.selectAllBarragesByID(vid);
                object[][] barrages = new object[proBarrages.Count()][];
                for (int i = 0; i < proBarrages.Count(); i++)
                {
                    BKBarrage barrage = proBarrages[i];
                    object[] objects = { barrage.videoTime, barrage.bType, barrage.color, barrage.uid, barrage.content };
                    barrages[i] = objects;
                }
                return Ok(CommonResult.Success(barrages, "Search Success"));
            }
            catch (Exception) {
                return Ok(CommonResult.Fail("Search Fail"));
            }
        }
    }
}


