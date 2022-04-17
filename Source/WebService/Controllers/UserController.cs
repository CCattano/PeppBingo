using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.User;
using Pepp.Web.Apps.Bingo.Infrastructure.Exceptions;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : BaseController<IUserAdapter>
    {
        private readonly IMapper _mapper;
        public UserController(IMapper mapper, IUserAdapter adapter) : base(adapter)
        {
            _mapper = mapper;
        }

        [TokenRequired]
        [HttpGet]
        public async Task<ActionResult<UserBM>> GetUser()
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE userBE = await base.Adapter.GetUser(token);
            UserBM userBM = _mapper.Map<UserBM>(userBE);
            return userBM;
        }

        [TokenRequired]
        [HttpGet]
        public async Task<ActionResult<List<UserBM>>> SearchUsersByName([FromQuery] string name)
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await base.Adapter.GetUser(token);
            if (!requestingUser.IsAdmin)
                throw new WebException(HttpStatusCode.Forbidden, "Non-Administrators cannot search for users");
            List<UserBE> userBEs = await base.Adapter.GetUsers(name);
            List<UserBM> userBMs = userBEs?.Select(userBE => _mapper.Map<UserBM>(userBE)).ToList();
            return userBMs;
        }
    }
}
