using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaikeAsp.Models;
using BaikeAsp.Dto;
using BaikeAsp.Dao;
using BaikeAsp.Util;
using BaikeAsp.Common;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkLoginController : ControllerBase
    {
        private readonly IUserReposity _userReposity;
        private readonly IUserInfoReposity _userInfoReposity;

        public BkLoginController(IUserReposity userReposity, IUserInfoReposity userInfoReposity)
        {
            _userReposity = userReposity ?? throw new ArgumentNullException(nameof(userReposity));
            _userInfoReposity = userInfoReposity ?? throw new ArgumentNullException(nameof(userInfoReposity));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] BKRegisterInfo info)
        {
            int count = await _userReposity.CheckUserByAccountAndNickNameAsync(info.Account, info.NickName);
            if (count > 0)
            {
                // 先这样写
                return Ok(CommonResult.Fail("account or nickName have been used")); 
            }
            string salt = "baike";
            BkUser user = new BkUser { Account = info.Account, Password = MD5Util.GenerateMD5(info.Password, salt), Salt = salt };
            BkUserInfo userInfo = new BkUserInfo { NickName = info.NickName, State = 1, Icon = "user_default.jpg", BackgroundIcon = "back_default.jpg" };
            user.BkUserInfo = userInfo;
            _userReposity.AddUser(user);
            await _userReposity.SaveAsync();
            return Ok(CommonResult.Success("register success"));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] BKLoginInfo loginInfo)
        {
            if(HttpContext.Session.GetInt32("userID") != null)
            {
                return Ok(CommonResult.Fail("already login"));
            }
            BkUser user = await _userReposity.GetUserByAccount(loginInfo.Account);
            if (user == null)
            {
                return Ok(CommonResult.Fail("e - mail address don't exist"));
            }
            if (await _userInfoReposity.GetState(user.UId) == 0)
            {
                return Ok(CommonResult.Fail("you have been banned, please contact administrator first"));
            }
            if (user.Password == MD5Util.GenerateMD5(loginInfo.Password, user.Salt))
            {
                HttpContext.Session.SetInt32("userID", user.UId);
                return Ok(CommonResult.Success(user.Account));
            }
            else
            {
                return Ok(CommonResult.Fail("password error"));
            }
        }

        [HttpPost("logout")]
        public void logout()
        {
            HttpContext.Session.Clear();
        }
    }
}
