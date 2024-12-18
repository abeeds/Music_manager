# Getting Started #

This solution contains an easy to use and simple music manager.

## Main Feature - Playlist Management
### Database
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

I also updated some entity relationships in `music-manager-start.Data\DataDbContext.cs`.

### API Endpoints
I then had to create the following endpoints to access the tables I had just created.
The following endpoints can be found in `Server/Controllers`.

Endpoints that manage the Playlists table:
- GET `/api/playlists`
    - Returns all playlists
- GET `/api/playlists?Id={PLAYLIST_ID}` 
    - Returns the specified playlist's data
    - Id must be a valid Guid
- POST `/api/playlists`
    - Sample JSON Request: `{"Title": "My Playlist", "Desc": "My favorite songs."}`
- PUT `/api/playlists` 
    - Updates a playlist's values
    - Id is required and must be a valid Guid
    - Must have atleast one of these values: Title and Desc
    - Sample JSON Request: `{"Id": "8213D28C-29EB-4365-8867-C721494BD30A", "Desc": "My favorite songs."}`
- DELETE `/api/playlists?Id={PLAYLIST_ID}`
    - Deletes the specified playlist
    - Id must be a valid Guid

Endpoints that manage the PlaylistSongs table:
- GET `/api/playlists/songs?=PlaylistId={PLAYLIST_ID}`
    - Returns all songs in the specified playlist
    - PlaylistId must be a valid Guid
- POST `/api/playlists/songs`
    - Adds a song to a playlist
    - Sample JSON Request: `{"PlaylistId": "8213D28C-29EB-4365-8867-C721494BD30A", "SongId": "22AA6F84-06D8-4A0E-BDAD-3000B35B5B5F"}`
- DELETE `/api/playlists/songs?=PlaylistId={PLAYLIST_ID}&SongId={SONG_ID}`
    - Removes a song from a playlist

### Frontend
The following new pages were created in the front end:
- `/{Id}`
    - Displays the details of a song with the specified id
    - - implemented in `SongDetails.razor`
- `/?query={}`
    - shows search results
    - implemented in `Index.razor`
- `/Playlists`
    - Displays all playlists available, how many songs they have,
    and their description
    - Each playlist has the following options
        - Edit - redirects to a form allowing the user to edit the playlist's title and description
        - Delete - this opens a confirmation window prompting the user 
        if they're sure they want to delete this playlist
    - implemented in `Playlists.razor`
- `/Playlists/{Id}`
    - Displays all songs in the specified playlist
    - implemented in `PlaylistSongs.razor`
- `/AddPlaylists`
    - Form that creates a new playlist
    - implemented in `AddPlaylist.razor`
- `/EditPlaylist/{Id}`
    - Form that updates a playlist's title and description
    - implemented in `EditPlaylist.razor`

Songs have an icon under their title that allows user to add add them to
playlists. Clicking this icon opens up a prompt that displays all playlists.
Clicking the checkbox next to the playlist title either adds or removes the
song from the playlist.

## Improvement 1 - Song Details
I implemented this by modifying the GET `/api/songs` endpoint to accept an
optional parameter, `Id`. If `Id` is provided, it will return the matching
song. In the frontend, I made each individual song redirect to a page with
the url `/{Id}` if they are clicked. At this new page, all of the song's
information is shown. This is implemented in `Client/Pages/SongDetail.razor`.

## Improvement 2 - Search Feature
I implemented this by modifying the GET `/api/songs` endpoint to accept an
optional parameter, `search`. If `search` is provided, it will return any
songs that contain the keywords provided in the search.

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


