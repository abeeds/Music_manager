﻿using music_manager_starter.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace music_manager_start.Data.Models
{
    internal class PlaylistSong
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
    }
}
