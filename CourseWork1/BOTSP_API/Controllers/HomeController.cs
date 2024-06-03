using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotifyAuthController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string client_id = "e3d3dc11e6de46e3bf8e69503bb38f66";
        private readonly string client_secret = "a3ee70ec0bc04502a6f72d3d0e18bfe5";
        private readonly string redirect_uri = "https://localhost:7099/Auth/callback";

        [HttpGet("callback")]
        public async Task<IActionResult> SpotifyCallback([FromQuery] string code, [FromQuery] string state)
        {
            if (state == null)
            {
                return BadRequest(new { error = "state_mismatch" });
            }
            else
            {
                var authOptions = new
                {
                    url = "https://accounts.spotify.com/api/token",
                    form = new
                    {
                        code,
                        redirect_uri,
                        grant_type = "authorization_code"
                    }
                };

                var basicAuthValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuthValue);
                var response = await client.PostAsync(authOptions.url, new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", authOptions.form.code),
                    new KeyValuePair<string, string>("redirect_uri", authOptions.form.redirect_uri),
                    new KeyValuePair<string, string>("grant_type", authOptions.form.grant_type)
                }));

                var responseString = await response.Content.ReadAsStringAsync();
                return Ok(responseString);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserProfile([FromQuery] string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "access_token_missing" });
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync("https://api.spotify.com/v1/me");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return Ok(responseString);
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }
        [HttpPost("createPlaylist")]
        public async Task<IActionResult> CreatePlaylist([FromQuery] string userId, [FromQuery] string accessToken, [FromBody] PlaylistInfo playlistInfo)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "user_id_or_access_token_missing" });
            }

            string url = $"https://api.spotify.com/v1/users/{userId}/playlists";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var playlistData = new
            {
                name = playlistInfo.Name,
                description = playlistInfo.Description,
                @public = playlistInfo.IsPublic
            };
            var content = new StringContent(JsonConvert.SerializeObject(playlistData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return Ok(responseString);
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }
        public class PlaylistInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsPublic { get; set; }
        }
        [HttpGet("searchTrack")]
        public async Task<IActionResult> SearchTrack([FromQuery] string query, [FromQuery] string accessToken)
        {
            if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "query_or_access_token_missing" });
            }

            string url = $"https://api.spotify.com/v1/search?q={HttpUtility.UrlEncode(query)}&type=track";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return Ok(responseData);
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }
        [HttpPost("addTrackToPlaylist")]
        public async Task<IActionResult> AddTrackToPlaylist([FromQuery] string playlistId, [FromQuery] string accessToken, [FromBody] List<string> trackUris)
        {
            if (string.IsNullOrEmpty(playlistId) || string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "playlist_id_or_access_token_missing" });
            }

            string url = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var data = new
            {
                uris = trackUris
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                return Ok(responseData);
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }
        [HttpDelete("removeTracksFromPlaylist")]
        public async Task<IActionResult> RemoveTracksFromPlaylist([FromQuery] string playlistId, [FromQuery] string accessToken, [FromBody] TrackUris trackUris)
        {
            if (string.IsNullOrEmpty(playlistId) || string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "playlist_id_or_access_token_missing" });
            }

            string url = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var data = new
            {
                tracks = trackUris.Tracks.Select(uri => new { uri }),
                snapshot_id = trackUris.SnapshotId
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = content
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }
        public class TrackUris
        {
            public List<string> Tracks { get; set; }
            public string SnapshotId { get; set; }
        }
        [HttpPut("updatePlaylistDetails")]
        public async Task<IActionResult> UpdatePlaylistDetails([FromQuery] string playlistId, [FromQuery] string accessToken, [FromBody] PlaylistDetails playlistDetails)
        {
            if (string.IsNullOrEmpty(playlistId) || string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "playlist_id_or_access_token_missing" });
            }

            string url = $"https://api.spotify.com/v1/playlists/{playlistId}";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var data = new
            {
                name = playlistDetails.Name,
                description = playlistDetails.Description,
                @public = playlistDetails.IsPublic
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }
        [HttpPost("nextTrack")]
        public async Task<IActionResult> NextTrack([FromQuery] string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "access_token_missing" });
            }

            string url = "https://api.spotify.com/v1/me/player/next";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }
        [HttpPost("previousTrack")]
        public async Task<IActionResult> PreviousTrack([FromQuery] string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "access_token_missing" });
            }

            string url = "https://api.spotify.com/v1/me/player/previous";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }

        [HttpPut("play")]
        public async Task<IActionResult> PlayTrack([FromQuery] string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "access_token_missing" });
            }

            string url = "https://api.spotify.com/v1/me/player/play";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }

        [HttpPut("pause")]
        public async Task<IActionResult> PauseTrack([FromQuery] string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest(new { error = "access_token_missing" });
            }

            string url = "https://api.spotify.com/v1/me/player/pause";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorString);
            }
        }

public class PlaylistDetails
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsPublic { get; set; }
        }

    }

}