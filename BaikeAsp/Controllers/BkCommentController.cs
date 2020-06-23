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
        private readonly ICommentReposity commentReposity;
        public BkCommentController(ICommentReposity comment)
        {
            commentReposity = comment ?? throw new ArgumentNullException(nameof(commentReposity));
        }

        [HttpPost("send/comment")]
        public async Task<ActionResult> sendComment([FromBody] BkComments comments)
        {
            comments.SendTime = new DateTime();
            try
            {
                int num = await commentReposity.insertComment(comments);
                if (num == 1)
                {
                    return Ok(CommonResult.Success("Send Success"));
                }
                return Ok(CommonResult.Success("Send Fail"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Send Fail"));
            }
        }
    }
}


