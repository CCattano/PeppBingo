using Microsoft.AspNetCore.Http;
using Pepp.Web.Apps.Bingo.Infrastructure.Caches;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Secrets =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch
{
    public interface ITwitchClient
    {
        /// <summary>
        /// Takes in the token received from Twitch to retrieve access token data
        /// </summary>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        Task<TwitchAccessToken> GetAccessToken(string accessCode);
        /// <summary>
        /// Uses the contents of an obsolete access token to retrieve new access token details
        /// </summary>
        /// <param name="tokenToRefresh"></param>
        /// <returns></returns>
        Task<TwitchAccessToken> RefreshAccessToken(TwitchAccessToken tokenToRefresh);
        /// <summary>
        /// Fetches User info from Twitch using the access token generated
        /// by the user when they authorized us to pull their info
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<TwitchUser> GetUser(string accessToken);
        /// <summary>
        /// Anticipated overload. Currently unused. Will throw exception if invoked.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="twitchUserId"></param>
        /// <returns></returns>
        Task GetUser(string accessToken, string twitchUserId);
    }

    public class TwitchClient : ITwitchClient
    {
        private readonly ITwitchCache _cache;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpCtx;

        public TwitchClient(
            ITwitchCache cache,
            HttpClient client, 
            IHttpContextAccessor httpCtx
        )
        {
            _cache = cache;
            _client = client;
            _httpCtx = httpCtx;
        }

        public async Task<TwitchAccessToken> GetAccessToken(string accessCode)
        {
            string clientID = _cache.GetApiSecret(Secrets.Types.Twitch.ClientID);
            string clientSecret = _cache.GetApiSecret(Secrets.Types.Twitch.ClientSecret);

            string redirectUri =
                $"{_httpCtx.HttpContext.Request.Scheme}://" +
                $"{_httpCtx.HttpContext.Request.Host}" +
                $"{_httpCtx.HttpContext.Request.Path}";

            Dictionary<string, string> queryParams = new()
            {
                { "client_id", clientID },
                { "client_secret", clientSecret },
                { "code", accessCode },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUri }
            };
            string queryParamsStr = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            const string baseUri = @"https://id.twitch.tv";
            string request = $"{baseUri}/{TwitchRequests.GetAccessToken}?{queryParamsStr}";
            using HttpResponseMessage response = await _client.PostAsync(request, null);
            
            TwitchAccessToken accessToken = null;
            if (response.IsSuccessStatusCode)
                accessToken = await response.Content.ReadFromJsonAsync<TwitchAccessToken>();
            return accessToken;
        }

        public async Task<TwitchAccessToken> RefreshAccessToken(TwitchAccessToken tokenToRefresh)
        {
            string clientID = _cache.GetApiSecret(Secrets.Types.Twitch.ClientID);
            string clientSecret = _cache.GetApiSecret(Secrets.Types.Twitch.ClientSecret);

            Dictionary<string, string> refreshRequestData = new()
            {
                { "client_id", clientID },
                { "client_secret", clientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", tokenToRefresh.RefreshToken }
            };
            FormUrlEncodedContent httpRequestContent = new(refreshRequestData);

            const string baseUri = @"https://id.twitch.tv";
            string request = $"{baseUri}/{TwitchRequests.GetAccessToken}";

            using HttpResponseMessage response = await _client.PostAsync(request, httpRequestContent);
            TwitchAccessToken accessToken = null;
            if (response.IsSuccessStatusCode)
                accessToken = await response.Content.ReadFromJsonAsync<TwitchAccessToken>();
            return accessToken;
        }

        public async Task<TwitchUser> GetUser(string accessToken)
        {
            string clientID = _cache.GetApiSecret(Secrets.Types.Twitch.ClientID);

            _client.DefaultRequestHeaders.Add("Client-ID", clientID);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            const string baseUri = @"https://api.twitch.tv";
            string request = $"{baseUri}/{TwitchRequests.GetUserData}";
            using HttpResponseMessage response = await _client.GetAsync(request);

            if (!response.IsSuccessStatusCode) return null;

            string responseData = await response.Content.ReadAsStringAsync();
            JsonElement jsonData = JsonDocument.Parse(responseData).RootElement;
            JsonElement userData = jsonData.GetProperty("data").EnumerateArray().SingleOrDefault();
            TwitchUser result = JsonSerializer.Deserialize<TwitchUser>(userData.GetRawText());
            return result;
        }

        public async Task GetUser(string accessToken, string twitchUserId)
        {
            await Task.FromException(new NotImplementedException());
        }

        private struct TwitchRequests
        {
            public const string GetAccessToken = "oauth2/token";
            public const string RefreshAccessToken = "oauth2/token";
            public const string GetUserData = "helix/users";
        }
    }
}
