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
using AutoMapper;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkCommentController : ControllerBase
    {
        private readonly ICommentReposity commentReposity;
        private readonly IMapper mapper;

        public BkCommentController(ICommentReposity commentReposity, IMapper mapper)
        {
            this.commentReposity = commentReposity ?? throw new ArgumentNullException(nameof(BkCommentController.commentReposity));
            this.mapper = mapper;
        }

        [HttpPost("send/comment")]
        public async Task<ActionResult> sendComment([FromBody] BKCommentsViewModel bKCommentsView)
        {
            bKCommentsView.SendTime = DateTime.Now;
            var comment = mapper.Map<BkComments>(bKCommentsView);
            commentReposity.AddComment(comment);
            await commentReposity.SaveAsync();
            return Ok(CommonResult.Success(bKCommentsView, "send successfully"));
        }
    }
}


