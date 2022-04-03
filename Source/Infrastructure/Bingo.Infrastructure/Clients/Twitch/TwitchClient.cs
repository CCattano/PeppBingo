﻿using Microsoft.AspNetCore.Http;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using Pepp.Web.Apps.Bingo.Infrastructure.Managers.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants
    .ValueDetails.Api.ApiSecrets.Types.Twitch.Types;

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
        private readonly ITwitchCacheManager _cacheManager;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpCtx;

        public TwitchClient(
            ITwitchCacheManager cacheManager,
            HttpClient client, 
            IHttpContextAccessor httpCtx
        )
        {
            _cacheManager = cacheManager;
            _client = client;
            _httpCtx = httpCtx;
        }

        public async Task<TwitchAccessToken> GetAccessToken(string accessCode)
        {
            string clientID = _cacheManager.GetApiSecret(TwitchTypes.ClientID);
            string clientSecret = _cacheManager.GetApiSecret(TwitchTypes.ClientSecret);

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

        public async Task<TwitchUser> GetUser(string accessToken)
        {
            string clientID = _cacheManager.GetApiSecret(TwitchTypes.ClientID);

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
            public const string GetUserData = "helix/users";
        }
    }
}