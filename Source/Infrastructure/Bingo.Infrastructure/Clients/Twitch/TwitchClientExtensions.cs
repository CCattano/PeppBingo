using Microsoft.Extensions.DependencyInjection;
using System;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch
{
    public static class TwitchClientExtensions
    {
        public static void AddTwitchClient(this IServiceCollection services)
        {
            services.AddHttpClient<ITwitchClient, TwitchClient>(client =>
            {
                client.BaseAddress = new Uri("https://id.twitch.tv");
            });
        }
    }
}
