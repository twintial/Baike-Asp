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
using Newtonsoft.Json.Linq;
using BaikeAsp.Dao.Impl;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkEditVideoController : ControllerBase
    {
        private readonly IVideoReposity videoReposity;
        private readonly IInteractiveVideoReposity interactiveVideoReposity;
        private readonly INextVideoReposity nextVideoReposity;
        public BkEditVideoController(IVideoReposity video, IInteractiveVideoReposity interactiveVideo, INextVideoReposity nextVideo)
        {
            videoReposity = video ?? throw new ArgumentNullException(nameof(videoReposity));
            interactiveVideoReposity = interactiveVideo ?? throw new ArgumentNullException(nameof(interactiveVideoReposity));
            nextVideoReposity = nextVideo ?? throw new ArgumentNullException(nameof(nextVideoReposity));
        }

        [HttpPost("edit")]
        public ActionResult getStructure([FromForm(Name = "ID")] int ivid, [FromForm(Name = "Structure")] string structure)
        {
            solveStructure(structure, ivid);
            return Ok(CommonResult.Success("Commit Success"));
        }

        [HttpGet("edit/{interVideoID}")]
        public async Task<ActionResult> getVideoListByInterVID([FromRoute] int interVideoID)
        {
            try
            {
                List<BKVideoViewModel> videos = await videoReposity.selectVideoByInterVID(interVideoID);
                return Ok(CommonResult.Success(videos, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Search Fail"));
            }
        }

        public async void solveStructure(string structure, int interVID)
        {
            //获取JSON字符串
            JObject jsonObject = JObject.Parse(structure);
            //获取起始视频
            string startVideoName = Convert.ToString(jsonObject["video"]);
            //获取起始视频ID
            int startVideoID = await returnVideoID(startVideoName, interVID);
            //更新表数据
            interactiveVideoReposity.updateInterVideoStartVideo(interVID, startVideoID);
            //检测视频是否已被编辑，如果已被编辑，先删除之前的数据
            deleteStructure(interVID);
            //向数据库插入视频结构信息
            getRelationship(jsonObject, interVID);
        }

        public async void deleteStructure(int interVID)
        {

            //获取interVideo的状态信息，为1则退出。为2则删除数据
            int state = await interactiveVideoReposity.selectInterVideoStateByID(interVID);
            if (state == 1)
            {
                return;
            }
            else
            {
                int initVideoID = (int)await interactiveVideoReposity.selectInitVideoIDByID(interVID);
                List<BkNextVideo> nextVideos = await nextVideoReposity.selectNextVideoByVideoID(initVideoID);
                if (nextVideos.Count() == 0)
                {
                    return;
                }
                for (int i = 0; i < nextVideos.Count(); i++)
                {
                    int nextVideoID = nextVideos[i].NextVideoId;
                    deleteRelationship(nextVideoID);
                }
                nextVideoReposity.deleteNextVideoByID(initVideoID);
                return;
            }
        }

        public async void deleteRelationship(int nextVideoID)
        {
            int nowVideoID = nextVideoID;
            List<BkNextVideo> nextVideos = await nextVideoReposity.selectNextVideoByVideoID(nowVideoID);
            if (nextVideos.Count() == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < nextVideos.Count(); i++)
                {
                    int next = nextVideos[i].NextVideoId;
                    deleteRelationship(next);
                }
                nextVideoReposity.deleteNextVideoByID(nowVideoID);
                return;
            }
        }

        public async void getRelationship(JObject jsonObject, int interVID)
        {

            //递归遍历JSON树，将分支选项和视频插入数据库
            if (jsonObject["children"] != null)
            {
                string nowVideoName = Convert.ToString(jsonObject["video"]);
                JArray jsonArray = JArray.Parse(jsonObject["children"].ToString());
                for (int i = 0; i < jsonArray.Count(); i++)
                {
                    JObject childJsonObject = JObject.Parse(jsonArray[i].ToString());
                    string childVideoName = Convert.ToString(childJsonObject["video"]);
                    string childVideoPlot = Convert.ToString(childJsonObject["plot"]);
                    int childVideoID = await returnVideoID(childVideoName, interVID);
                    int nowVideoID = await returnVideoID(nowVideoName, interVID);

                    BkNextVideo bkNextVideo = new BkNextVideo();
                    bkNextVideo.VideoId = nowVideoID;
                    bkNextVideo.NextVideoId = childVideoID;
                    bkNextVideo.Choice = childVideoPlot;

                    nextVideoReposity.insertNextVideo(bkNextVideo);

                    getRelationship(childJsonObject, interVID);
                }
            }
            else
            {
                return;
            }
        }

        public async Task<int> returnVideoID(string videoName, int interVID)
        {

            //返回JSON中的视频ID信息
            List<BKVideoViewModel> videos = await videoReposity.selectVideoByInterVID(interVID);
            for (int i = 0; i < videos.Count(); i++)
            {
                if (videos[i].title.Equals(videoName))
                {
                    return videos[i].videoID;
                }
            }
            return -1;
        }
    }
}


