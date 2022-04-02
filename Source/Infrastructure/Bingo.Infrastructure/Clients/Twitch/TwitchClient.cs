using Microsoft.AspNetCore.Http;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
    }

    public class TwitchClient : ITwitchClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpCtx;

        // TODO setup CacheManager to hold content and init the cache key contents in Startup
        public TwitchClient(HttpClient client, IHttpContextAccessor httpCtx)
        {
            _client = client;
            _httpCtx = httpCtx;
        }

        public async Task<TwitchAccessToken> GetAccessToken(string accessCode)
        {
            // TODO setup storage for these in the Db and a caching system for fetching them
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            string redirectUri =
                $"{_httpCtx.HttpContext.Request.Scheme}://" +
                $"{_httpCtx.HttpContext.Request.Host}" +
                $"{_httpCtx.HttpContext.Request.Path}";

            Dictionary<string, string> queryParams = new()
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", accessCode },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUri }
            };
            string queryParamsStr = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            
            string request = $"{TwitchRequests.GetAccessToken}?{queryParamsStr}";
            using HttpResponseMessage response = await _client.PostAsync(request, null);
            
            TwitchAccessToken accessToken = null;
            if (response.IsSuccessStatusCode)
                accessToken = await response.Content.ReadFromJsonAsync<TwitchAccessToken>();
            return accessToken;
        }

        private struct TwitchRequests
        {
            public const string GetAccessToken = "oauth2/token";
        }
    }
}
