using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaikeAsp.Models;
using BaikeAsp.Dto;
using BaikeAsp.Dao;
using BaikeAsp.Util;
using System.Runtime.InteropServices;

namespace BaikeAsp.Controllers
{
    [Route("api")]
    [ApiController]
    public class BkUsersController : ControllerBase
    {
        private readonly IUserReposity _userReposity;

        public BkUsersController(IUserReposity userReposity)
        {
            _userReposity = userReposity ?? throw new ArgumentNullException(nameof(userReposity));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] BKRegisterInfo info)
        {
            int count = await _userReposity.CheckUserByAccountAndNickNameAsync(info.Account, info.NickName);
            if (count > 0)
            {
                // 先这样写
                return BadRequest(new { error = "acount or nickName have been used" });
            }
            string salt = "baike";
            BkUser user = new BkUser { Account = info.Account, Password = MD5Util.GenerateMD5(info.Password, salt), Salt = salt };
            BkUserInfo userInfo = new BkUserInfo { NickName = info.NickName, State = 1, Icon = "user_default.jpg", BackgroundIcon = "back_default.jpg" };
            user.BkUserInfo = userInfo;
            _userReposity.AddUser(user);
            await _userReposity.SaveAsync();
            return Ok("注册成功");
        }

        //// GET: api/BkUsers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BkUser>>> GetBkUser()
        //{
        //    return await _context.BkUser.ToListAsync();
        //}

        //// GET: api/BkUsers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<BkUser>> GetBkUser(int id)
        //{
        //    var bkUser = await _context.BkUser.FindAsync(id);

        //    if (bkUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return bkUser;
        //}

        //// PUT: api/BkUsers/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBkUser(int id, BkUser bkUser)
        //{
        //    if (id != bkUser.UId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(bkUser).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BkUserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/BkUsers
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<BkUser>> PostBkUser(BkUser bkUser)
        //{
        //    _context.BkUser.Add(bkUser);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBkUser", new { id = bkUser.UId }, bkUser);
        //}

        //// DELETE: api/BkUsers/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<BkUser>> DeleteBkUser(int id)
        //{
        //    var bkUser = await _context.BkUser.FindAsync(id);
        //    if (bkUser == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.BkUser.Remove(bkUser);
        //    await _context.SaveChangesAsync();

        //    return bkUser;
        //}

        //private bool BkUserExists(int id)
        //{
        //    return _context.BkUser.Any(e => e.UId == id);
        //}
    }
}
