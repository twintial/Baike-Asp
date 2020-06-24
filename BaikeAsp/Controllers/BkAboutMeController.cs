using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BaikeAsp.Dto;
using BaikeAsp.Dao;
using BaikeAsp.Util;
using BaikeAsp.Common;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkAboutMeController : ControllerBase
    {
        private readonly ICollectionReposity collectionReposity;
        private readonly IFavouriteReposity favouriteReposity;
        private readonly IUserInfoReposity userInfoReposity;
        private readonly IInteractiveVideoReposity interactiveVideoReposity;
        private readonly IBrowseHistoryReposity browseHistoryReposity;

        public BkAboutMeController(ICollectionReposity collection, IFavouriteReposity favourite,
            IUserInfoReposity userInfo, IInteractiveVideoReposity interactiveVideo, IBrowseHistoryReposity browseHistory)
        {
            collectionReposity = collection ?? throw new ArgumentNullException(nameof(collectionReposity));
            favouriteReposity = favourite ?? throw new ArgumentNullException(nameof(favouriteReposity));
            userInfoReposity = userInfo ?? throw new ArgumentNullException(nameof(userInfoReposity));
            interactiveVideoReposity = interactiveVideo ?? throw new ArgumentNullException(nameof(interactiveVideoReposity));
            browseHistoryReposity = browseHistory ?? throw new ArgumentNullException(nameof(browseHistoryReposity));
        }

        [HttpPost("aboutMe")]
        public async Task<ActionResult> getUserInfomation()
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            BKHeadInfoViewModel bkHeadInfoViewModel = new BKHeadInfoViewModel();
            try
            {
                BKUserInfoViewModel bkUserInfoViewModel = await userInfoReposity.GetBkUserInfo((int)uid);
                int uploadVideoNum = await interactiveVideoReposity.getUploadVideoNum((int)uid);
                int favVideoNum = await collectionReposity.getFavVideoNum((int)uid);
                int userFollowerNum = await favouriteReposity.getUserFollowerNum((int)uid);
                int usersFollowNum = await favouriteReposity.getUsersFollowNum((int)uid);

                //bkHeadInfoViewModel.iconURL = bkUserInfoViewModel.iconURL;
                //bkHeadInfoViewModel.state = bkUserInfoViewModel.state;
                //bkHeadInfoViewModel.introduction = bkUserInfoViewModel.introduction;
                //bkHeadInfoViewModel.backgroundIconURL = bkUserInfoViewModel.backgroundIconURL;
                //bkHeadInfoViewModel.nickName = bkUserInfoViewModel.nickName;
                //bkHeadInfoViewModel.uID = bkUserInfoViewModel.uID;
                bkHeadInfoViewModel.U = bkUserInfoViewModel;

                bkHeadInfoViewModel.uploadVideoNum = uploadVideoNum;
                bkHeadInfoViewModel.favVideoNum = favVideoNum;
                bkHeadInfoViewModel.userFollowerNum = userFollowerNum;
                bkHeadInfoViewModel.usersFollowNum = usersFollowNum;
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
            return Ok(CommonResult.Success(bkHeadInfoViewModel, "Search Success"));
        }

        [HttpGet("getLoginUserInfo/{vID}")]
        public async Task<ActionResult> getLoginUserInfo([FromRoute] int vID)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            BKUserInfoViewModel bkUserInfo = await userInfoReposity.GetBkUserInfo((int)uid);
            long colNum = await collectionReposity.isCollect((int)uid, vID);
            return Ok(CommonResult.Success(bkUserInfo, colNum.ToString()));
        }

        [HttpGet("aboutMe/favVideo/{pageNum}")]
        public async Task<ActionResult> getFavVideoByPage([FromRoute] int pageNum)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            PagedList<BKInteractiveVideoViewModel> result = await interactiveVideoReposity.selectFavVideoByUid((int)uid, pageNum, 5);

            return Ok(CommonResult.Success(result, "Search Success"));
        }

        [HttpDelete("aboutMe/favVideo/{interVideoID}")]
        public async Task<ActionResult> deleteFavVideo([FromRoute] int interVideoID)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            try
            {
                await collectionReposity.deleteFavVideoByID((int)uid, interVideoID);
                await collectionReposity.SaveAsync();
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Delete Fail"));
            }
            return Ok(CommonResult.Success("Delete Success"));
        }

        [HttpGet("aboutMe/userFollowers/{pageNum}")]
        public async Task<ActionResult> getUserFollowersByPage([FromRoute] int pageNum)
        {
            int? uid = (int)HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }

            try
            {
                PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUserFollowersByUid((int)uid, pageNum, 12);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Search Fail"));
            }
        }

        [HttpGet("aboutMe/usersFollow/{pageNum}")]
        public async Task<ActionResult> getUsersFollowByPage([FromRoute] int pageNum)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }

            PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUsersFollowByUid((int)uid, pageNum, 12);

            return Ok(CommonResult.Success(result, "Search Success"));
        }

        [HttpPost("aboutMe/setting")]
        public async Task<ActionResult> updateUserInfo([FromBody] BKUpdateUserInfo userInfo)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            try
            {
                await userInfoReposity.updateUserInforByID((int)uid, userInfo.nickName, userInfo.introduction);
                await userInfoReposity.SaveAsync();
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Update Fail"));
            }
            return Ok(CommonResult.Success("Update Success"));
        }

        [HttpDelete("aboutMe/usersFollow/{unFollowID}")]
        public async Task<ActionResult> deleteUsersFollow([FromRoute] int unFollowID)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            try
            {
                await favouriteReposity.deleteUsersFollowByID((int)uid, unFollowID);
                await favouriteReposity.SaveAsync();
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Delete Fail"));
            }
            return Ok(CommonResult.Success("Delete Success"));
        }

        [HttpGet("aboutHis/{oID}")]
        public async Task<ActionResult> getOUserInfomation([FromRoute] int oID)
        {
            BKHeadInfoViewModel bkHeadInfoViewModel = new BKHeadInfoViewModel();
            try
            {
                BKUserInfoViewModel bkUserInfoViewModel = await userInfoReposity.GetBkUserInfo(oID);
                int uploadVideoNum = await interactiveVideoReposity.getUploadVideoNum(oID);
                int favVideoNum = await collectionReposity.getFavVideoNum(oID);
                int userFollowerNum = await favouriteReposity.getUserFollowerNum(oID);
                int usersFollowNum = await favouriteReposity.getUsersFollowNum(oID);

                //bkHeadInfoViewModel.iconURL = bkUserInfoViewModel.iconURL;
                //bkHeadInfoViewModel.state = bkUserInfoViewModel.state;
                //bkHeadInfoViewModel.introduction = bkUserInfoViewModel.introduction;
                //bkHeadInfoViewModel.backgroundIconURL = bkUserInfoViewModel.backgroundIconURL;
                //bkHeadInfoViewModel.nickName = bkUserInfoViewModel.nickName;
                //bkHeadInfoViewModel.uID = bkUserInfoViewModel.uID;
                bkHeadInfoViewModel.U = bkUserInfoViewModel;

                bkHeadInfoViewModel.uploadVideoNum = uploadVideoNum;
                bkHeadInfoViewModel.favVideoNum = favVideoNum;
                bkHeadInfoViewModel.userFollowerNum = userFollowerNum;
                bkHeadInfoViewModel.usersFollowNum = usersFollowNum;
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
            return Ok(CommonResult.Success(bkHeadInfoViewModel, "Search Success"));
        }

        [HttpGet("aboutHis/favVideo/{oID}/{pageNum}")]
        public async Task<ActionResult> getOUserFavVideoByPage([FromRoute] int oID, [FromRoute] int pageNum)
        {
            try
            {
                PagedList<BKInteractiveVideoViewModel> result = await interactiveVideoReposity.selectFavVideoByUid(oID, pageNum, 5);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutHis/userFollowers/{oID}/{pageNum}")]
        public async Task<ActionResult> getOUserFollowersByPage([FromRoute] int oID, [FromRoute] int pageNum)
        {
            try
            {
                PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUserFollowersByUid(oID, pageNum, 12);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutHis/usersFollow/{oID}/{pageNum}")]
        public async Task<ActionResult> getOUsersFollowByPage([FromRoute] int oID, [FromRoute] int pageNum)
        {
            try
            {
                PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUsersFollowByUid(oID, pageNum, 12);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutHis/subscribe/{oID}")]
        public async Task<ActionResult> subscribeFollowerByID([FromRoute] int oID)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            if (oID.Equals(uid))
            {
                return Ok(CommonResult.Fail("You can not subscribed yourself"));
            }
            try
            {
                favouriteReposity.insertUsersFollowByID((int)uid, oID);
                await favouriteReposity.SaveAsync();
                return Ok(CommonResult.Success("Update Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("You have subscribed"));
            }
        }

        [HttpGet("aboutHis/hisVideo/{oID}/{pageNum}")]
        public async Task<ActionResult> getHisVideoByPage([FromRoute] int oID, [FromRoute] int pageNum)
        {
            try
            {
                PagedList<BKInteractiveVideoViewModel> result = await interactiveVideoReposity.selectHisVideoByUid(oID, pageNum, 5);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("You have collected"));
            }
        }

        [HttpGet("aboutHis/FavHisVideo/{vID}")]
        public async Task<ActionResult> favVideo([FromRoute] int vID)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            try
            {
                collectionReposity.insertFavVideoByID((int)uid, vID);
                await collectionReposity.SaveAsync();
                return Ok(CommonResult.Success("Update Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutMe/browseHistory/{pageNum}")]
        public async Task<ActionResult> getBrowseHistoryByPage([FromRoute] int pageNum)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            try

            {
                var result = await interactiveVideoReposity.selectBrowseHistoryByUid((int)uid, pageNum, 5);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpDelete("aboutMe/browseHistory/{interVideoID}")]
        public async Task<ActionResult> deleteBrowseHistory([FromRoute] int interVideoID)
        {
            int? uid = HttpContext.Session.GetInt32("userID");
            if (uid == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            try
            {
                await browseHistoryReposity.deleteBrowseHistoryByID((int)uid, interVideoID);
                await browseHistoryReposity.SaveAsync();
                return Ok(CommonResult.Success("Update Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpPost("aboutMe/upload")]
        public async Task<ActionResult> coverUpload(
            [FromForm(Name = "avatar")] IFormFile file,
            [FromForm(Name = "UserID")] int? uID,
            [FromForm(Name = "MyIcon")] string IconID)
        {
            if (uID == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            string iconPath = Path.Combine(ResourcePath.USER_ICON, uID.ToString());
            Directory.CreateDirectory(iconPath);
            // 删除老图
            if (!IconID.Equals("user_default.jpg"))
            {
                System.IO.File.Delete(Path.Combine(ResourcePath.USER_ICON, IconID));
            }
            // 放入新图
            var suffix = Path.GetExtension(file.FileName);
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            using (FileStream fs = System.IO.File.Create($@"{iconPath}\{uuid}{suffix}"))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            // 更新数据库
            await userInfoReposity.updateUserIconByID((int)uID, $@"{uID}\{uuid}{suffix}");
            await userInfoReposity.SaveAsync();
            return Ok("Upload Icon Success");
        }

        [HttpPost("aboutMe/uploadback")]
        public async Task<ActionResult> BackImgUpload(
            [FromForm(Name = "backavatar")] IFormFile file,
            [FromForm(Name = "BackUserID")] int? uID,
            [FromForm(Name = "BackMyIcon")] string IconID)
        {
            if (uID == null)
            {
                return Ok(CommonResult.Fail("Please Login First"));
            }
            string iconPath = Path.Combine(ResourcePath.USER_ICON, uID.ToString());
            Directory.CreateDirectory(iconPath);
            // 删除老图
            if (!IconID.Equals("back_default.jpg"))
            {
                System.IO.File.Delete(Path.Combine(ResourcePath.USER_ICON, IconID.Replace(@"/", @"\")));
            }
            // 放入新图
            var suffix = Path.GetExtension(file.FileName);
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            using (FileStream fs = System.IO.File.Create($@"{iconPath}\{uuid}{suffix}"))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            // 更新数据库
            await userInfoReposity.updateUserBackgroundIconByID((int)uID, $@"{uID}/{uuid}{suffix}");
            await userInfoReposity.SaveAsync();
            return Ok("Upload Icon Success");
        }
    }
}
