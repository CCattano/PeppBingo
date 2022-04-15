using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Pepp.Web.Apps.Bingo.Data;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using Pepp.Web.Apps.Bingo.Infrastructure.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiSecretSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Sources;
using JWTSecretTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.TokenSecrets.Types.JWT;
using TokenSecretSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.TokenSecrets.Sources;
using TwitchSecretTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Types.Twitch;

namespace Pepp.Web.Apps.Bingo.WebService.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CacheHydrationMiddleware
    {
        private readonly RequestDelegate _next;

        public CacheHydrationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IBingoDataService dataSvc, ITwitchCache twitchCache, ITokenCache tokenCache)
        {
            await VerifyTwitchAPISecretsCache(dataSvc, twitchCache);
            await VerifyTokenSecretsCache(dataSvc, tokenCache);
            await _next(httpContext);
        }

        private static async Task VerifyTwitchAPISecretsCache(IBingoDataService dataSvc, ITwitchCache cache)
        {
            string clientID = cache.GetApiSecret(TwitchSecretTypes.ClientID);
            string clientSecret = cache.GetApiSecret(TwitchSecretTypes.ClientSecret);

            if (!string.IsNullOrWhiteSpace(clientID) && !string.IsNullOrWhiteSpace(clientSecret))
                return;

            List<SecretEntity> valueDetailDescriptions =
               await dataSvc.Api.SecretRepo.GetSecrets(ApiSecretSources.Twitch);

            Dictionary<TwitchSecretTypes, string> apiSecretsByType =
                valueDetailDescriptions.ToDictionary(key => Enum.Parse<TwitchSecretTypes>(key.Type), value => value.Value);

            cache.SetApiSecret(TwitchSecretTypes.ClientID, apiSecretsByType[TwitchSecretTypes.ClientID]);
            cache.SetApiSecret(TwitchSecretTypes.ClientSecret, apiSecretsByType[TwitchSecretTypes.ClientSecret]);
        }

        private static async Task VerifyTokenSecretsCache(IBingoDataService dataSvc, ITokenCache cache)
        {
            string sha256Key = cache.GetTokenSecret(JWTSecretTypes.SHA256Key);
            string signingSecret = cache.GetTokenSecret(JWTSecretTypes.SigningSecret);

            if (!string.IsNullOrWhiteSpace(sha256Key) && !string.IsNullOrWhiteSpace(signingSecret))
                return;

            List<SecretEntity> valueDetailDescriptions =
               await dataSvc.Token.SecretRepo.GetSecrets(TokenSecretSources.JWT);

            Dictionary<JWTSecretTypes, string> apiSecretsByType =
                valueDetailDescriptions.ToDictionary(key => Enum.Parse<JWTSecretTypes>(key.Type), value => value.Value);

            cache.SetTokenSecret(JWTSecretTypes.SHA256Key, apiSecretsByType[JWTSecretTypes.SHA256Key]);
            cache.SetTokenSecret(JWTSecretTypes.SigningSecret, apiSecretsByType[JWTSecretTypes.SigningSecret]);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CacheHydrationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheHydrationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheHydrationMiddleware>();
        }
    }
}
