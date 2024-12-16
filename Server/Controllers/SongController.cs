using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_manager_starter.Data;
using music_manager_starter.Data.Models;
using System;

namespace music_manager_starter.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly DataDbContext _context;

        public SongsController(DataDbContext context)
        {
            _context = context;
        }

  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs(Guid? Id = null)
        {
            // return single song if an Id is given
            if (Id.HasValue)
            {
                var s = await _context.Songs
                    .Where(s => s.Id == Id)
                    .FirstOrDefaultAsync();

                if (s == null)
                    return NotFound("Song not found.");

                return Ok(s);
            }

            // return all songs if no Id is given
            return await _context.Songs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            if (song == null)
                return BadRequest("Song cannot be null.");

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
