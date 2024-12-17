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
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs(Guid? Id = null, string? search = null)
        {
            // return single song if an Id is given
            if (Id.HasValue)
            {
                var s = await _context.Songs.FindAsync(Id);
                if (s == null)
                    return NotFound("Song not found.");

                return Ok(s);
            }

            // return song matching search parameters
            if (search != null) 
            {
                search = search.ToLower();
                string[] keywords = search.Split(' ');
                var songs = _context.Songs.AsQueryable();

                foreach (var word in keywords)
                {
                    songs = songs.Where(song => song.Title.ToLower().Contains(word)
                        || song.Album.ToLower().Contains(word)
                        || song.Artist.ToLower().Contains(word));
                }
                
                return await songs.ToListAsync();
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
