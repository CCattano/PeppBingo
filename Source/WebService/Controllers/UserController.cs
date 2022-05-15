using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.User;
using Pepp.Web.Apps.Bingo.Infrastructure.Enums;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [TokenRequired]
    [Route("[controller]/[action]")]
    public class UserController : BaseController<IMapper, IUserAdapter>
    {
        public UserController(IMapper mapper, IUserAdapter adapter) : base(mapper, adapter)
        {
        }

        [HttpGet]
        public async Task<ActionResult<UserBM>> GetUser()
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE userBE = await base.Adapter.GetUser(token);
            UserBM userBM = base.Mapper.Map<UserBM>(userBE);
            return userBM;
        }

        [HttpPost]
        public async Task<ActionResult> LogSuspiciousBehaviour()
        {
            UserBE requestingUser = await GetRequestingUser();
            base.Adapter.LogSuspiciousUserBehaviour(requestingUser.UserID);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<UserSubmissionStatus>> GetUserSubmissionStatus()
        {
            UserBE requestingUser = await GetRequestingUser();
            UserSubmissionStatus userCanSubmitBingo = base.Adapter.GetUserSubmissionStatus(requestingUser.UserID);
            return Ok(JsonSerializer.Serialize(userCanSubmitBingo));
        }
        
        [HttpPost]
        public async Task<ActionResult> MarkUserAsBingoSubmitted()
        {
            UserBE requestingUser = await GetRequestingUser();
            base.Adapter.MarkUserAsBingoSubmitted(requestingUser.UserID);
            return Ok();
        }
        
        private async Task<UserBE> GetRequestingUser()
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await base.Adapter.GetUser(token);
            return requestingUser;
        }
    }
}
