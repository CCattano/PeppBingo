using Microsoft.Extensions.DependencyInjection;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch
{
    public static class TwitchClientExtensions
    {
        public static void AddTwitchClient(this IServiceCollection services)
        {
            services.AddHttpClient<ITwitchClient, TwitchClient>();
        }
    }
}
