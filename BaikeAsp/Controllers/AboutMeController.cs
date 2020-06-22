﻿using System;
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
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;

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
        private readonly IBrowseHistoryReposity browseHistoryReposity;

        public AboutMeController(ICollectionReposity collection, IFavouriteReposity favourite,
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
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            BkHeadInfoViewModel bkHeadInfoViewModel = new BkHeadInfoViewModel();
            try
            {
                BkUserInfo bkUserInfo = await userInfoReposity.GetBkUserInfo(uid);
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
            return Ok(CommonResult.Success(bkHeadInfoViewModel, "Search Success"));
        }

        [HttpGet("getLoginUserInfo/{vID}")]
        public async Task<ActionResult> getLoginUserInfo([FromRoute] int vID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            BkUserInfo bkUserInfo = await userInfoReposity.GetBkUserInfo(uid);
            long colNum = await collectionReposity.isCollect(uid, vID);
            return Ok(CommonResult.Success(bkUserInfo, colNum.ToString()));
        }

        [HttpGet("aboutMe/favVideo/{pageNum}")]
        public async Task<ActionResult> getFavVideoByPage([FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            PagedList<BkInteractiveVideo> result = await interactiveVideoReposity.selectFavVideoByUid(uid, pageNum, 5);

            return Ok(CommonResult.Success(result, "Search Success"));
        }

        [HttpDelete("aboutMe/favVideo/{interVideoID}")]
        public async Task<ActionResult> deleteFavVideo([FromRoute] int interVideoID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            try
            {
                collectionReposity.deleteFavVideoByID(uid, interVideoID);
                await collectionReposity.SaveAsync();
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Delete Fail"));
            }
            return Ok(CommonResult.Success("Delete Success"));
        }

        [HttpGet("aboutMe/userFollowers/{pageNum}")]
        public async Task<ActionResult> getUserFollowersByPage([FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }

            try
            {
                PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUserFollowersByUid(uid, pageNum, 12);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Search Fail"));
            }
        }

        [HttpGet("aboutMe/usersFollow/{pageNum}")]
        public async Task<ActionResult> getUsersFollowByPage([FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }

            PagedList<BKUserFollowersDto> result = await userInfoReposity.selectUsersFollowByUid(uid, pageNum, 12);

            return Ok(CommonResult.Success(result, "Search Success"));
        }

        [HttpPost("aboutMe/setting")]
        public async Task<ActionResult> updateUserInfo([FromBody] BKUpdateUserInfo userInfo)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            try
            {
                userInfoReposity.updateUserInforByID(uid, userInfo.NickName, userInfo.Introduction);
                await userInfoReposity.SaveAsync();
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Update Fail"));
            }
            return Ok(CommonResult.Success("Update Success"));
        }

        [HttpDelete("aboutMe/usersFollow/{unFollowID}")]
        public async Task<ActionResult> deleteUsersFollow([FromRoute] int unFollowID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            try
            {
                favouriteReposity.deleteUsersFollowByID(uid, unFollowID);
                await favouriteReposity.SaveAsync();
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Delete Fail"));
            }
            return Ok(CommonResult.Success("Delete Success"));
        }

        [HttpGet("aboutHis/{oID}")]
        public async Task<ActionResult> getOUserInfomation([FromRoute] int oID)
        {
            try
            {
                BkUserInfo bkUserInfo = await userInfoReposity.GetBkUserInfo(oID);
                return Ok(CommonResult.Success(bkUserInfo, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutHis/favVideo/{oID}/{pageNum}")]
        public async Task<ActionResult> getOUserFavVideoByPage([FromRoute] int oID, [FromRoute] int pageNum)
        {
            try
            {
                PagedList<BkInteractiveVideo> result = await interactiveVideoReposity.selectFavVideoByUid(oID, pageNum, 5);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
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
                return NotFound(CommonResult.Fail("Unknown Error"));
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
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutHis/subscribe/{oID}")]
        public async Task<ActionResult> subscribeFollowerByID([FromRoute] int oID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            if (oID.Equals(uid))
            {
                return NotFound(CommonResult.Fail("You can not subscribed yourself"));
            }
            try
            {
                favouriteReposity.insertUsersFollowByID(uid, oID);
                await favouriteReposity.SaveAsync();
                return Ok(CommonResult.Success("Update Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutHis/hisVideo/{oID}/{pageNum}")]
        public async Task<ActionResult> getHisVideoByPage([FromRoute] int oID, [FromRoute] int pageNum)
        {
            try
            {
                PagedList<BkInteractiveVideo> result = await interactiveVideoReposity.selectHisVideoByUid(oID, pageNum, 5);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutHis/FavHisVideo/{vID}")]
        public async Task<ActionResult> favVideo([FromRoute] int vID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            try
            {
                collectionReposity.insertFavVideoByID(uid, vID);
                await collectionReposity.SaveAsync();
                return Ok(CommonResult.Success("Update Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpGet("aboutMe/browseHistory/{pageNum}")]
        public async Task<ActionResult> getBrowseHistoryByPage([FromRoute] int pageNum)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            try

            {
                PagedList<BkInteractiveVideo> result = await interactiveVideoReposity.selectHisVideoByUid(uid, pageNum, 5);
                return Ok(CommonResult.Success(result, "Search Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
        }

        [HttpDelete("aboutMe/browseHistory/{interVideoID}")]
        public async Task<ActionResult> deleteBrowseHistory([FromRoute] int interVideoID)
        {
            int uid = (int)HttpContext.Session.GetInt32("userID");
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return NotFound(CommonResult.Fail("Please Login First"));
            }
            try
            {
                browseHistoryReposity.deleteBrowseHistoryByID(uid, interVideoID);
                await browseHistoryReposity.SaveAsync();
                return Ok(CommonResult.Success("Update Success"));
            }
            catch (Exception)
            {
                return NotFound(CommonResult.Fail("Unknown Error"));
            }
        }

        //[HttpPost("aboutMe/upload")]
        //public async Task<ActionResult> coverUpload()
        //{

        //    var boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(Request.ContentType).Boundary).Value;

        //    var reader = new MultipartReader(boundary, HttpContext.Request.Body);

        //    var section = await reader.ReadNextSectionAsync();

        //    while (section != null)
        //    {
        //        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
        //        if (hasContentDispositionHeader)
        //        {
        //            var trustedFileNameForFileStorage = Path.GetRandomFileName();
        //            await WriteFileAsync(section.Body, Path.Combine(ResourcePath.tempURL, trustedFileNameForFileStorage));
        //        }
        //        section = await reader.ReadNextSectionAsync();
        //    }
        //    return Created(nameof(FileController), null);



        //    log.info("play");
        //    if (UserID == null)
        //    {
        //        log.info("no user");
        //        return ResultFactory.buildFailResult("Please Login First");
        //    }
        //    if (!file.isEmpty())
        //    {
        //        if (file.getContentType() != null && file.getOriginalFilename() != null)
        //        {
        //            String fileName = file.getOriginalFilename();
        //            String type = fileName.substring(fileName.lastIndexOf("."));
        //            String uuid = UUID.randomUUID().toString().replace("-", "");
        //            String path = baseURL + "img" + File.separator + "userIcon" + File.separator + UserID + File.separator + uuid + type;
        //            File dest = new File(path);
        //            //判断文件父目录是否存在
        //            if (!dest.getParentFile().exists())
        //            {
        //                dest.getParentFile().mkdir();
        //            }
        //            file.transferTo(dest);
        //            String final_path = UserID + File.separator + uuid + type;
        //            try
        //            {
        //                log.info("start");
        //                aboutMeMapper.updateUsersIconByID(UserID, final_path);
        //                if (!IconID.equals("user_default.jpg"))
        //                {
        //                    deleteFile(new File(baseURL + "src/resources/img" + File.separator + "userIcon" + File.separator + IconID));
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                return ResultFactory.buildFailResult("Unexpected Error");
        //            }
        //            return ResultFactory.buildSuccessResult("Upload Icon Success");
        //        }
        //    }
        //    return ResultFactory.buildFailResult("Upload Icon Fail");
        //}


        //public static async Task<string> WriteFileAsync(Stream stream, string path, int fileSize)
        //{
        //    int FILE_WRITE_SIZE = fileSize;
        //    int writeCount = 0;
        //    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, FILE_WRITE_SIZE, true))
        //    {
        //        byte[] byteArr = new byte[FILE_WRITE_SIZE];
        //        int readCount = 0;
        //        while ((readCount = await stream.ReadAsync(byteArr, 0, byteArr.Length)) > 0)
        //        {
        //            await fileStream.WriteAsync(byteArr, 0, readCount);
        //            writeCount += readCount;
        //        }
        //    }
        //    return path;
        //}
    }
}