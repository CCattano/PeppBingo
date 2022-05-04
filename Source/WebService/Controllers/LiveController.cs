using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Infrastructure.Managers;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [TokenRequired]
    [Route("Admin/[controller]/[action]")]
    public class LiveController : BaseController
    {
        private readonly IUserAdapter _userAdapter;
        private readonly ILiveAdapter _liveAdapter;

        public LiveController(
            IUserAdapter userAdapter,
            ILiveAdapter liveAdapter
        )
        {
            _userAdapter = userAdapter;
            _liveAdapter = liveAdapter;
        }

        [HttpPut]
        public async Task<ActionResult> SetActiveBoardID([FromQuery] int activeBoardID)
        {
            await ConfirmIsAdmin("Non-Administrators cannot access live controls");
            await _liveAdapter.SetActiveBoardID(activeBoardID);
            return Ok();
        }

        private async Task ConfirmIsAdmin(string rejectionMsg)
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await _userAdapter.GetUser(token);
            if (!requestingUser.IsAdmin)
                throw new WebException(HttpStatusCode.Forbidden, rejectionMsg);
        }
    }
}
