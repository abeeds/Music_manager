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
            var playlist = await _context.Playlists.FindAsync(PlaylistId);
            if(playlist == null)
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
            var playlist = await _context.Playlists.FindAsync(PlaylistSong.PlaylistId);
            if(playlist == null)
                return NotFound("Playlist not found.");

            // ensure song exists
            var s = await _context.Songs.FindAsync(PlaylistSong.SongId);
            if (s == null)
                return NotFound("Song not found.");

            // check if song is already in playlist
            var pls = await _context.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == PlaylistSong.PlaylistId && ps.SongId == PlaylistSong.SongId);
            if(pls != null)
                return Conflict("Song is already in playlist.");

            _context.PlaylistSongs.Add(PlaylistSong);
            playlist.SongCount += 1;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<PlaylistSong>> DelPlaylistSong(Guid PlaylistId, Guid SongId)
        {
            // ensure playlist exists
            var playlist = await _context.Playlists.FindAsync(PlaylistId);
            if(playlist == null)
                return NotFound("Playlist not found.");

            // check if the entry exists
            var pls = await _context.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == PlaylistId && ps.SongId == SongId);
            if(pls == null)
                return NotFound("Song not found in playlist.");

            _context.PlaylistSongs.Remove(pls);
            playlist.SongCount -= 1;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
