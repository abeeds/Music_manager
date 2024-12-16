using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_manager_starter.Data;
using music_manager_starter.Data.Models;
using System;
using System.Text.Json;

namespace music_manager_starter.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly DataDbContext _context;

        public PlaylistsController(DataDbContext context)
        {
            _context = context;
        }

        // expected body of PutPlaylist
        public class EditPlaylist
        {
            public Guid Id { get; set; }
            public string? Title { get; set; } = null;
            public string? Desc { get; set; } = null;
        }

        // Retrieves playlist data
        // If Id Param is included, it will only include that playlist's data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists(Guid? Id = null)
        {
            // search for specific Playlist if an Id was provided
            if (Id.HasValue)
            {
                var pl = await _context.Playlists.FindAsync(Id);
                if (pl == null) 
                    return NotFound("Playlist not found.");

                return Ok(pl);
            }

            // return all playlists
            return await _context.Playlists.ToListAsync();
        }

        // Creates a new playlist
        [HttpPost]
        public async Task<ActionResult<Playlist>> PostPlaylist(Playlist playlist)
        {
            if (playlist == null)
                return BadRequest("Playlist cannot be null.");

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Edit an existing playlist, which is specified by Id in the json
        // need to have atleast one of Title or Desc in the json
        [HttpPut]
        public async Task<ActionResult<Playlist>> PutPlaylist(EditPlaylist playlist)
        {
            // check arguments
            if(playlist == null)
                return BadRequest("Playlist cannot be null.");
            if(playlist.Id == Guid.Empty) 
                return BadRequest("Playlist Id required.");
            if(playlist.Title == null && playlist.Desc == null)
                return BadRequest("Must include atleast one: Title, Desc.");

            // ensure playlist exists
            var existing_pl = await _context.Playlists.FindAsync(playlist.Id);
            if(existing_pl == null)
                return NotFound("Playlist not found.");

            // update values
            if(playlist.Title != null)
                existing_pl.Title = playlist.Title;
            if(playlist.Desc != null)
                existing_pl.Desc = playlist.Desc;

            await _context.SaveChangesAsync();
            return Ok("Playlist updated.");
        }

        [HttpDelete]
        public async Task<ActionResult<Playlist>> DelPlaylist(Guid Id)
        {
            if(Id == Guid.Empty)
                return BadRequest("Id cannot be null.");

            var pl = await _context.Playlists.FindAsync(Id);
            if(pl == null)
                return NotFound("Playlist not found.");

            _context.Playlists.Remove(pl);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
