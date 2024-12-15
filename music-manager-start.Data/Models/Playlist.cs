using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace music_manager_starter.Data.Models
{
    public sealed class Playlist
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int SongCount { get; set; }

        public ICollection<PlaylistSong> PlaylistSong { get; } = new List<PlaylistSong>();
    }
}
