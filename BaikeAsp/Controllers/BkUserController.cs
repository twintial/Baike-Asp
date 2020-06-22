using BaikeAsp.Dao;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkUserController : ControllerBase
    {
        private readonly IUserReposity _userReposity;
        private readonly IUserInfoReposity _userInfoReposity;

        public BkUserController(IUserReposity userReposity, IUserInfoReposity userInfoReposity)
        {
            _userReposity = userReposity ?? throw new ArgumentNullException(nameof(userReposity));
            _userInfoReposity = userInfoReposity ?? throw new ArgumentNullException(nameof(userInfoReposity));
        }
        
        [HttpGet("SearchUser/{title}/{page}")]
        public async Task<ActionResult> Search(string title, string page)
        {
            var l = await _userReposity.SearchUsers(title);
            return Ok();
        }
    }
}
