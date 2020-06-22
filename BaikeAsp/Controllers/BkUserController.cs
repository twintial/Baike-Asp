using AutoMapper;
using BaikeAsp.Common;
using BaikeAsp.Dao;
using BaikeAsp.Dao.Impl;
using BaikeAsp.Dto;
using BaikeAsp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkUserController : ControllerBase
    {
        private readonly IUserReposity _userReposity;
        private readonly IUserInfoReposity _userInfoReposity;
        private readonly IFavouriteReposity _favouriteReposity;
        private readonly ICollectionReposity _collectionReposity;
        private readonly IInteractiveVideoReposity _interactiveVideoReposity;
        private readonly IMapper _mapper;

        public BkUserController(
            IUserReposity userReposity,
            IUserInfoReposity userInfoReposity,
            IFavouriteReposity favouriteReposity,
            ICollectionReposity collectionReposity,
            IInteractiveVideoReposity interactiveVideoReposity,
            IMapper mapper)
        {
            _userReposity = userReposity ?? throw new ArgumentNullException(nameof(userReposity));
            _userInfoReposity = userInfoReposity ?? throw new ArgumentNullException(nameof(userInfoReposity));
            _favouriteReposity = favouriteReposity;
            _collectionReposity = collectionReposity;
            _interactiveVideoReposity = interactiveVideoReposity;
            _mapper = mapper;
        }
        
        [HttpGet("SearchUser/{title}/{page}")]
        public async Task<ActionResult> Search(string title, int page)
        {
            var users = await _userReposity.SearchUsers(title, page, 4);
            List<int> followNum = new List<int>();
            List<int> videoNum = new List<int>();
            List<BkUserInfo> infos = new List<BkUserInfo>();
            foreach (var user in users)
            {
                followNum.Add(_favouriteReposity.GetFollowerCountByUid(user.UId));
                videoNum.Add(_interactiveVideoReposity.GetVideoCountByUid(user.UId));
                infos.Add(user.BkUserInfo);
            }

            var userSearchViewModels = _mapper.Map<List<BKSearchUser>>(infos);
            int count = await _userReposity.GetCount(title);

            BKSearchUserListViewModel result = new BKSearchUserListViewModel
            {
                List = userSearchViewModels, 
                PageNum = count, 
                Follow = followNum, 
                Video = videoNum 
            };
            
            return Ok(result);
        }

        [HttpGet("SearchUser/{page}")]
        public async Task<ActionResult> SearchAll(int page)
        {
            var users = await _userReposity.SearchAllUsers(page, 4);
            List<int> followNum = new List<int>();
            List<int> videoNum = new List<int>();
            List<BkUserInfo> infos = new List<BkUserInfo>();
            foreach (var user in users)
            {
                followNum.Add(_favouriteReposity.GetFollowerCountByUid(user.UId));
                videoNum.Add(_interactiveVideoReposity.GetVideoCountByUid(user.UId));
                infos.Add(user.BkUserInfo);
            }

            var userSearchViewModels = _mapper.Map<List<BKSearchUser>>(infos);

            int count = await _userReposity.GetCount(null);

            BKSearchUserListViewModel result = new BKSearchUserListViewModel
            {
                List = userSearchViewModels,
                PageNum = count,
                Follow = followNum,
                Video = videoNum
            };

            return Ok(result);
        }
    }
}
