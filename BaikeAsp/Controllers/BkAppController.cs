﻿using System;
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
    public class BkAppController : ControllerBase
    {
        private readonly IUserInfoReposity userInfoReposity;
        public BkAppController(IUserInfoReposity userInfo)
        {
            userInfoReposity = userInfo ?? throw new ArgumentNullException(nameof(userInfoReposity));
        }

        [HttpPost("isOnline")]
        public int Test()
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return -1;
            }
            else
            {
                return uid;
            }
        }

        [HttpGet("getname/{uid}")]
        public async Task<ActionResult> Getname([FromRoute] int uid)
        {
            try
            {
                BkUserInfo bkUserInfo = await userInfoReposity.GetBkUserInfo(uid);
                return Ok(CommonResult.Success(bkUserInfo, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Search Fail"));
            }
        }
    }
}

