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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists(Guid? Id = null)
        {
            // search for specific Playlist if an Id was provided
            if (Id.HasValue)
            {
                var pl = await _context.Playlists
                    .Where(p => p.Id == Id)
                    .ToListAsync();

                if (pl.Count == 0) 
                {
                    return NotFound("Playlist not found.");
                }
                return Ok(pl);
            }

            // return all playlists
            return await _context.Playlists.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Playlist>> PostPlaylist(Playlist playlist)
        {
            if (playlist == null)
            {
                return BadRequest("Playlist cannot be null.");
            }

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
