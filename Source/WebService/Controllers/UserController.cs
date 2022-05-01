using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.User;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : BaseController<IMapper, IUserAdapter>
    {
        public UserController(IMapper mapper, IUserAdapter adapter) : base(mapper, adapter)
        {
        }

        [TokenRequired]
        [HttpGet]
        public async Task<ActionResult<UserBM>> GetUser()
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE userBE = await base.Adapter.GetUser(token);
            UserBM userBM = base.Mapper.Map<UserBM>(userBE);
            return userBM;
        }
    }
}
