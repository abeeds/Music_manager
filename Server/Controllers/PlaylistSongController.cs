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

        public PlaylistSongsController(DataDbContext context)
        {
            _context = context;
        }

        // returns songs from the specified playlist
        // also returns that playlists data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetPlaylistSongs(Guid PlaylistId)
        {
            if(PlaylistId == Guid.Empty)
                return BadRequest("PlaylistId cannot be null.");

            // ensure playlist exists
            var playlist = await _context.Playlists
                .Where(pl => pl.Id == PlaylistId)
                .ToListAsync();
            if(playlist.Count == 0)
                return NotFound("Playlist not found.");

            // get all songs in the playlist
            var plsongs = await _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == PlaylistId)
                .Join(
                    _context.Songs,
                    playlistSong => playlistSong.SongId,
                    song => song.Id,
                    (playlistSong, song) => new
                    {
                        song.Id,
                        song.Title,
                        song.Artist,
                        song.Album,
                        song.Genre
                    }
                )
                .ToListAsync();

            if (plsongs.Count == 0)
                return NotFound("No songs are in the specified playlist.");

            return Ok(plsongs);
        }

        [HttpPost]
        public async Task<ActionResult<Song>> PostPlaylistSong(PlaylistSong PlaylistSong)
        {
            // ensure fields are correct
            if (PlaylistSong == null
                || PlaylistSong.PlaylistId == Guid.Empty
                || PlaylistSong.SongId == Guid.Empty
            )
                return BadRequest("PlaylistId and SongId cannot be null.");

            // ensure playlist exists
            var playlist = await _context.Playlists
                .Where(pl => pl.Id == PlaylistSong.PlaylistId)
                .ToListAsync();
            if(playlist.Count == 0)
                return NotFound("Playlist not found.");

            // ensure song exists
            var s = await _context.Songs
                .Where(s => s.Id == PlaylistSong.SongId)
                .ToListAsync();
            if (s.Count == 0)
                return NotFound("Song not found.");

            // check if song is already in playlist
            var pls = await _context.PlaylistSongs
                .Where(pls => pls.PlaylistId == PlaylistSong.PlaylistId 
                              && pls.SongId == PlaylistSong.SongId)
                .ToListAsync();
            if(pls.Count != 0)
                return Conflict("Song is already in playlist.");

            _context.PlaylistSongs.Add(PlaylistSong);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
