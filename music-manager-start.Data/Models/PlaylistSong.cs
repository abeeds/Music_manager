﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_manager_starter.Data.Models
{
    public sealed class PlaylistSong
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }

        // relationships
        public Playlist Playlist { get; set; } = null!;
        public Song Song { get; set; } = null!;
    }
}
