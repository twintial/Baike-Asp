using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BaikeAsp.Models;
using BaikeAsp.Dto;
using BaikeAsp.Dao;
using BaikeAsp.Util;
using BaikeAsp.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class AboutMeController : ControllerBase
    {
        private readonly ICollectionReposity collectionReposity;
        private readonly IFavouriteReposity favouriteReposity;
        private readonly IUserInfoReposity userInfoReposity;
        private readonly IInteractiveVideoReposity interactiveVideoReposity;

        public AboutMeController(ICollectionReposity collection, IFavouriteReposity favourite,
            IUserInfoReposity userInfo, IInteractiveVideoReposity interactiveVideo)
        {
            collectionReposity = collection ?? throw new ArgumentNullException(nameof(collectionReposity));
            favouriteReposity = favourite ?? throw new ArgumentNullException(nameof(favouriteReposity));
            userInfoReposity = userInfo ?? throw new ArgumentNullException(nameof(userInfoReposity));
            interactiveVideoReposity = interactiveVideo ?? throw new ArgumentNullException(nameof(interactiveVideoReposity));
        }

        [HttpPost("aboutMe")]
        public async Task<ActionResult> getUserInfomation()
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            BkHeadInfoViewModel bkHeadInfoViewModel = new BkHeadInfoViewModel();
            try
            {
                IEnumerable<BkUserInfo> bkUserInfoSet = await userInfoReposity.GetBkUserInfo(uid);
                BkUserInfo bkUserInfo = bkUserInfoSet.First();
                int uploadVideoNum = await interactiveVideoReposity.getUploadVideoNum(uid);
                int favVideoNum = await collectionReposity.getFavVideoNum(uid);
                int userFollowerNum = await favouriteReposity.getUserFollowerNum(uid);
                int usersFollowNum = await favouriteReposity.getUsersFollowNum(uid);

                bkHeadInfoViewModel.U = bkUserInfo;
                bkHeadInfoViewModel.uploadVideoNum = uploadVideoNum;
                bkHeadInfoViewModel.favVideoNum = favVideoNum;
                bkHeadInfoViewModel.userFollowerNum = userFollowerNum;
                bkHeadInfoViewModel.usersFollowNum = usersFollowNum;
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
            return Ok(CommonResult.Success(bkHeadInfoViewModel,"Search Success"));
        }

        [HttpGet("getLoginUserInfo/{vID}")]
        public async Task<ActionResult> getLoginUserInfo([FromRoute] int vID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            IEnumerable<BkUserInfo> bkUserInfoSet = await userInfoReposity.GetBkUserInfo(uid);
            BkUserInfo bkUserInfo = bkUserInfoSet.First();
            long colNum = await collectionReposity.isCollect(uid, vID);
            return Ok(CommonResult.Success(bkUserInfo, colNum.ToString()));
        }

        [HttpGet("/aboutMe/favVideo/{pageNum}")]
        public async Task<ActionResult> getFavVideoByPage([FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            PagedList<BkInteractiveVideo> result = await interactiveVideoReposity.selectFavVideoByUid(uid, pageNum, 5);

            return Ok(CommonResult.Success(result, "Search Success"));
        }

        [HttpDelete("/aboutMe/favVideo/{interVideoID}")]
        public async Task<ActionResult> deleteFavVideo([FromRoute] int interVideoID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            try
            {
                collectionReposity.deleteFavVideoByID(uid, interVideoID);
                await collectionReposity.SaveAsync();
            }
            catch (Exception e)
            {
                return NotFound(CommonResult.Fail("Delete Fail"));
            }
            return Ok(CommonResult.Success("Delete Success"));
        }

        [HttpGet("/aboutMe/userFollowers/{pageNum}")]
        public async Task<ActionResult> getUserFollowersByPage([FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");

            PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUserFollowersByUid(uid, pageNum, 12);

            return Ok(CommonResult.Success(result, "Search Success"));
        }

        [HttpGet("/aboutMe/usersFollow/{pageNum}")]
        public async Task<ActionResult> getUsersFollowByPage([FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");

            PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUsersFollowByUid(uid, pageNum, 12);

            return Ok(CommonResult.Success(result, "Search Success"));
        }
    }
}
