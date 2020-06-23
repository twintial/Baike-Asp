using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BaikeAsp.Dto;
using BaikeAsp.Common;
using BaikeAsp.Dao;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkAdminsController : ControllerBase
    {
        private readonly IInteractiveVideoReposity interactiveVideoReposity;
        private readonly IUserInfoReposity userInfoReposity;
        private readonly IAdminReposity adminReposity;

        public BkAdminsController(IInteractiveVideoReposity interactiveVideo, IUserInfoReposity userInfo, IAdminReposity admin){
            interactiveVideoReposity = interactiveVideo ?? throw new ArgumentNullException(nameof(interactiveVideoReposity));
            userInfoReposity = userInfo ?? throw new ArgumentNullException(nameof(userInfoReposity));
            adminReposity = admin ?? throw new ArgumentNullException(nameof(adminReposity));
        }

        [HttpGet("SearchVideoByAdmin/{title}/{page}/{searchStyle}/{tag}")]
        public async Task<BKSearchVideoListViewModel> Search([FromRoute] string title, [FromRoute] int page,
            [FromRoute] string searchStyle, [FromRoute] string tag)
        {
            BKSearchVideoListViewModel y = new BKSearchVideoListViewModel();
            y.pageNum = page;
            int state = 0;
            if (tag.Equals("NORMAL"))
            {
                state = 2;
            }
            try
            {
                switch (searchStyle)
                {
                    case "Collect":
                        y.list = await interactiveVideoReposity.selectByCollectVolume(title, state, page, 9);
                        break;
                    case "playV":
                        y.list = await interactiveVideoReposity.selectByPlayVolume(title, state, page, 9);
                        break;
                    default:
                        y.list = await interactiveVideoReposity.selectByTime(title, state, page, 9);
                        break;
                }
                return y;
            }
            catch (Exception)
            {
                return y;
            }
        }

        [HttpGet("SearchUserByAdmin/{title}/{page}/{searchStyle}/{tag}")]
        public async Task<BKSearchUserByAdministrationViewModel> Search2([FromRoute] string title, [FromRoute] int page,
            [FromRoute] string searchStyle, [FromRoute] string tag)
        {
            BKSearchUserByAdministrationViewModel y = new BKSearchUserByAdministrationViewModel();
            y.pageNum = page;
            int state = 0;
            if (tag.Equals("NORMAL"))
            {
                state = 1;
            }
            try
            {
                switch (searchStyle)
                {
                    case "name":
                        y.list = await userInfoReposity.selectByName(title, state, page, 9);
                        break;
                    default:
                        y.list = await userInfoReposity.selectByTime(title, state, page, 9);
                        break;
                }
                return y;
            }
            catch (Exception)
            {
                return y;
            }
        }

        [HttpGet("SearchVideoByAdmin/{page}/{searchStyle}/{tag}")]
        public async Task<BKSearchVideoListViewModel> Search3([FromRoute] int page, [FromRoute] string searchStyle, [FromRoute] string tag)
        {
            string title = "";
            BKSearchVideoListViewModel y = new BKSearchVideoListViewModel();
            y.pageNum = page;
            int state = 0;
            if (tag.Equals("NORMAL"))
            {
                state = 2;
            }
            try
            {
                switch (searchStyle)
                {
                    case "Collect":
                        y.list = await interactiveVideoReposity.selectByCollectVolume(title, state, page, 9);
                        break;
                    case "playV":
                        y.list = await interactiveVideoReposity.selectByPlayVolume(title, state, page, 9);
                        break;
                    default:
                        y.list = await interactiveVideoReposity.selectByTime(title, state, page, 9);
                        break;
                }
                return y;
            }
            catch (Exception)
            {
                return y;
            }
        }

        [HttpGet("SearchUserByAdmin/{page}/{searchStyle}/{tag}")]
        public async Task<BKSearchUserByAdministrationViewModel> Search4([FromRoute] int page, [FromRoute] string searchStyle, [FromRoute] string tag)
        {
            string title = "";
            BKSearchUserByAdministrationViewModel y = new BKSearchUserByAdministrationViewModel();
            y.pageNum = page;
            int state = 0;
            if (tag.Equals("NORMAL"))
            {
                state = 1;
            }
            try
            {
                switch (searchStyle)
                {
                    case "name":
                        y.list = await userInfoReposity.selectByName(title, state, page, 9);
                        break;
                    default:
                        y.list = await userInfoReposity.selectByTime(title, state, page, 9);
                        break;
                }
                return y;
            }
            catch (Exception)
            {
                return y;
            }
        }

        [HttpPut("ChangeUserState/{id}")]
        public async void ChangeUserState([FromRoute] int id)
        {
            try
            {
                userInfoReposity.changeUserState(id);
                await userInfoReposity.SaveAsync();
            }
            catch (Exception)
            {

            }
        }

        [HttpPut("ChangeVideoState/{id}")]
        public async void ChangeVideoState([FromRoute] int id)
        {
            try
            {
                interactiveVideoReposity.changeVideoState(id);
                await interactiveVideoReposity.SaveAsync();
            }
            catch (Exception)
            {

            }
        }

        [HttpPost("AdminLogin")]
        public string AdminLogin([FromBody] BKAdminViewModel bkAdmin)
        {
            if (bkAdmin == null)
            {
                return "false";
            }
            string account = bkAdmin.account;
            string psd = bkAdmin.password;
            try
            {
                if (adminReposity.Detection(account, psd) != null)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception)
            {
                return "false";
            }
        }
    }
}
