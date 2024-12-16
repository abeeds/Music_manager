# Getting Started #

This solution contains an easy to use and simple music manager.

## Agenda
- Big feature
    - write endpoints for playlist
        - edit playlist
        - delete playlist
    - write endpoints to playlist-song relationship
        - add to playlist
        - remove from playlist
        - get all songs from a playlist

- Search functionality
    - edit GET api/song to take a search query optionally
        - split words in the query
        - if any words match a title, album, or artist, return them

- Song details
    - clicking a song on the root page expands the box and shows details

- Redesign
    - copy spotify

## Main Feature - Playlist Management
The first feature I added to this is playlist management. To allow for
playlist management, I had to create a way to store playlists and their
associated songs to the database. I decided to go with the design shown
below.

![ERD](https://i.imgur.com/wrWHQ0X.png)

My though process for this was that a playlist can have multiple songs,
and a song can be in multiple playlists. To handle this many-to-many
relationship, I used PlaylistSong as an intermediary to help avoid
any redundancy in the database.

I implemented these in the following files:
- `music-manager-start.Data/Models/Playlist.cs`
- `music-manager-start.Data/Models/PlaylistSong.cs`

I also updated some entity relationships in `music-manager-start.Data\DataDbContext.cs`

I then had to create the following endpoints in the API to allow the frontend
to access these tables:
- GET `/api/playlists`
    - Returns all playlists
- GET `/api/playlists?id={PLAYLIST_ID}` 
    - Returns the specified playlists data
    - Id must be a valid Guid
- POST `/api/playlists`
    - Sample JSON Request: `{"Title": "My Playlist", "Desc": "My favorite songs."}`
- PUT `/api/playlists` 
    - Updates a playlist's values
    - Id is required and must be a valid Guid
    - Must have atleast one of these values: Title and Desc
    - Sample JSON Request: `{"Id": "abc-123-abc", "Desc": "My favorite songs."}`
- DELETE `/api/playlists?id={PLAYLIST_ID}`
    - Deletes the specified playlist
    - Id must be a valid Guid


## Improvement 1 - Song Details


## Improvement 2 - Search Feature


## Technologies
- Visual Studio 2022 
- .NET 8 SDK
- Node.js (for Tailwind CSS)
- Git
- EntityFramework Core 
- Blazor


## Local Enviroment Setup
You need to have the .NET 8 SDK installed. Be sure to download the latest version for Visual Studio 2022.

Use the latest version of Visual Studio 2022: https://visualstudio.microsoft.com/downloads/

Install the node packages before building the solution by using ```npm install``


