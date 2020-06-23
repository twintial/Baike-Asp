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
    public class BkCommentController : ControllerBase
    {
        private readonly IBarrageReposity barrageReposity;
        public BkCommentController(IBarrageReposity barrage)
        {
            barrageReposity = barrage ?? throw new ArgumentNullException(nameof(barrageReposity));
        }

        //[HttpPost("send/comment")]
        //public Task<ActionResult> sendComment()
        //{
        //    try
        //    {
        //        return Ok(CommonResult.Success("Send Success"));
        //    }
        //    catch (Exception)
        //    {
        //        return NotFound(CommonResult.Fail("Send Fail"));
        //    }
        //}
    }
}


