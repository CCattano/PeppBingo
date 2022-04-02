using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.Infrastructure.Clients.Twitch.Models;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class TwitchController : Controller
    {
        private readonly ITwitchAdapter _adapter;

        public TwitchController(ITwitchAdapter adapter)
        {
            _adapter = adapter;
        }

        /// <summary>
        /// Endpoint called by Twitch to provide the access_code of 
        /// a user who has authorized us to fetch their user data
        /// </summary>
        /// <param name="code">
        ///     The access_code that represents the user who has authorized us to pull their information./
        ///     <br /> This param name is dictated by Twitch API docs and CANNOT BE RENAMED
        /// </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<TwitchAccessToken>> AccessCode([FromQuery] string code)
        {
            TwitchAccessToken accessToken = await _adapter.GetAccessToken(code);
            return accessToken;
        }
    }
}
