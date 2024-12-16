using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_manager_starter.Data;
using music_manager_starter.Data.Models;
using System;

namespace music_manager_starter.Server.Controllers
{
    [Route("api/playlists/songs")]
    [ApiController]
    public class PlaylistSongsController : ControllerBase
    {
        private readonly DataDbContext _context;

        public PlaylistSongController(DataDbContext context)
        {
            _context = context;
        }

  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetPlaylistSongs(Guid PlaylistId = null)
        {
            if(PlaylistId == Guid.Empty)
                return BadRequest("PlaylistId cannot be null");
            var pls = await _context.PlaylistSongs
                .Where(pls => pls.PlaylistId == PlaylistId)
                .ToListAsync();

            if (s.Count == 0)
                return NotFound("No songs are in the specified playlist");

            return Ok(pls);

        }

        [HttpPost]
        public async Task<ActionResult<Song>> PostPlaylistSong(Song song)
        {
            if (song == null)
                return BadRequest("Song cannot be null.");

            _context.PlaylistSongs.Add(song);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
