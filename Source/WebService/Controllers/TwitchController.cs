using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class TwitchController : BaseController<ITwitchAdapter>
    {
        public TwitchController(ITwitchAdapter adapter) : base(adapter)
        {
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
        public async Task<ActionResult> AccessCode([FromQuery] string code)
        {
            string token = await base.Adapter.ProcessReceivedAccessCode(code);
            base.SetJWTCookieHeader(token);
            return RedirectPermanentPreserveMethod(base.BaseUri);
        }
    }
}
