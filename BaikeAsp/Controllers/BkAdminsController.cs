using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaikeAsp.Models;

namespace BaikeAsp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BkAdminsController : ControllerBase
    {
        private readonly BaikeContext _context;

        public BkAdminsController(BaikeContext context)
        {
            _context = context;
        }

        // GET: api/BkAdmins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BkAdmin>>> GetBkAdmin()
        {
            return await _context.BkAdmin.ToListAsync();
        }

        // GET: api/BkAdmins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BkAdmin>> GetBkAdmin(int id)
        {
            var bkAdmin = await _context.BkAdmin.FindAsync(id);

            if (bkAdmin == null)
            {
                return NotFound();
            }

            return bkAdmin;
        }

        // PUT: api/BkAdmins/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBkAdmin(int id, BkAdmin bkAdmin)
        {
            if (id != bkAdmin.AId)
            {
                return BadRequest();
            }

            _context.Entry(bkAdmin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BkAdminExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BkAdmins
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BkAdmin>> PostBkAdmin(BkAdmin bkAdmin)
        {
            _context.BkAdmin.Add(bkAdmin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBkAdmin", new { id = bkAdmin.AId }, bkAdmin);
        }

        // DELETE: api/BkAdmins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BkAdmin>> DeleteBkAdmin(int id)
        {
            var bkAdmin = await _context.BkAdmin.FindAsync(id);
            if (bkAdmin == null)
            {
                return NotFound();
            }

            _context.BkAdmin.Remove(bkAdmin);
            await _context.SaveChangesAsync();

            return bkAdmin;
        }

        private bool BkAdminExists(int id)
        {
            return _context.BkAdmin.Any(e => e.AId == id);
        }
    }
}
