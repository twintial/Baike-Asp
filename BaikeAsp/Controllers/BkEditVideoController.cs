using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BaikeAsp.Models;
using BaikeAsp.Dto;
using BaikeAsp.Common;
using BaikeAsp.Dao;
using Newtonsoft.Json.Linq;

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
        public ActionResult getVideoListByInterVID([FromRoute] int interVideoID)
        {
            try
            {
                List<BKVideoViewModel> videos = videoReposity.selectVideoByInterVID(interVideoID);
                return Ok(CommonResult.Success(videos, "Search Success"));
            }
            catch (Exception)
            {
                return Ok(CommonResult.Fail("Search Fail"));
            }
        }

        public void solveStructure(string structure, int interVID)
        {
            //获取JSON字符串
            JObject jsonObject = JObject.Parse(structure);
            //获取起始视频
            string startVideoName = Convert.ToString(jsonObject["video"]);
            //获取起始视频ID
            int startVideoID = returnVideoID(startVideoName, interVID);
            //更新表数据
            interactiveVideoReposity.updateInterVideoStartVideo(interVID, startVideoID);
            interactiveVideoReposity.Save();
            //检测视频是否已被编辑，如果已被编辑，先删除之前的数据
            deleteStructure(interVID);
            //向数据库插入视频结构信息
            getRelationship(jsonObject, interVID);
        }

        public bool deleteStructure(int interVID)
        {

            //获取interVideo的状态信息，为1则退出。为2则删除数据
            int state = interactiveVideoReposity.selectInterVideoStateByID_T(interVID);
            if (state == 1)
            {
                return true;
            }
            else
            {
                int initVideoID = (int)interactiveVideoReposity.selectInitVideoIDByID_T(interVID);
                List<BkNextVideo> nextVideos = nextVideoReposity.selectNextVideoByVideoID_T(initVideoID);
                if (nextVideos.Count() == 0)
                {
                    return true;
                }
                for (int i = 0; i < nextVideos.Count(); i++)
                {
                    int nextVideoID = nextVideos[i].NextVideoId;
                    deleteRelationship(nextVideoID);
                }
                nextVideoReposity.deleteNextVideoByID_T(initVideoID);
                nextVideoReposity.Save();
                return true;
            }
        }

        public bool deleteRelationship(int nextVideoID)
        {
            int nowVideoID = nextVideoID;
            List<BkNextVideo> nextVideos = nextVideoReposity.selectNextVideoByVideoID_T(nowVideoID);
            if (nextVideos.Count() == 0)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < nextVideos.Count(); i++)
                {
                    int next = nextVideos[i].NextVideoId;
                    deleteRelationship(next);
                }
                nextVideoReposity.deleteNextVideoByID_T(nowVideoID);
                nextVideoReposity.Save();
                return true;
            }
        }

        public bool getRelationship(JObject jsonObject, int interVID)
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
                    int childVideoID = returnVideoID(childVideoName, interVID);
                    int nowVideoID = returnVideoID(nowVideoName, interVID);

                    BkNextVideo bkNextVideo = new BkNextVideo();
                    bkNextVideo.VideoId = nowVideoID;
                    bkNextVideo.NextVideoId = childVideoID;
                    bkNextVideo.Choice = childVideoPlot;

                    nextVideoReposity.insertNextVideo_T(bkNextVideo);
                    nextVideoReposity.Save();

                    getRelationship(childJsonObject, interVID);
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public int returnVideoID(string videoName, int interVID)
        {
            //返回JSON中的视频ID信息
            List<BKVideoViewModel> videos = videoReposity.selectVideoByInterVID(interVID);
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


