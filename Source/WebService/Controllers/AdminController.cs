using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.Game;
using Pepp.Web.Apps.Bingo.BusinessModels.User;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [TokenRequired]
    [Route("[controller]/[action]")]
    public class AdminController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserAdapter _userAdapter;
        private readonly IGameAdapter _gameAdapter;

        public AdminController
        (
            IMapper mapper, 
            IUserAdapter userAdapter, 
            IGameAdapter gameAdapter
        )
        {
            _mapper = mapper;
            _userAdapter = userAdapter;
            _gameAdapter = gameAdapter;
        }

        #region Admin User Data Endpoints

        [HttpGet]
        public async Task<ActionResult<List<UserBM>>> SearchUsersByName([FromQuery] string name)
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await _userAdapter.GetUser(token);
            if (!requestingUser.IsAdmin)
                throw new WebException(HttpStatusCode.Forbidden, "Non-Administrators cannot search for users");
            List<UserBE> userBEs = await _userAdapter.GetUsers(name);
            List<UserBM> userBMs = userBEs?.Select(userBE => _mapper.Map<UserBM>(userBE)).ToList();
            return userBMs;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserBM>>> Admins()
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await _userAdapter.GetUser(token);
            if (!requestingUser.IsAdmin)
                throw new WebException(HttpStatusCode.Forbidden, "Non-Administrators cannot search for users");
            List<UserBE> userBEs = await _userAdapter.GetAdminUsers();
            List<UserBM> userBMs = userBEs?.Select(userBE => _mapper.Map<UserBM>(userBE)).ToList();
            return userBMs;
        }

        [HttpPut]
        public async Task<ActionResult> GrantAdminPermission([FromBody] int userID)
        {
            await ModifyAdminPermissionForUser(userID, true);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> RevokeAdminPermission([FromBody] int userID)
        {
            await ModifyAdminPermissionForUser(userID, false);
            return Ok();
        }

        private async Task ModifyAdminPermissionForUser(int userID, bool isAdmin)
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await _userAdapter.GetUser(token);
            if (!requestingUser.IsAdmin)
                throw new WebException(HttpStatusCode.Forbidden, "Non-Administrators cannot modify permissions of other users");
            await _userAdapter.SetAdminPermissionForUser(userID, isAdmin);
        }

        #endregion

        #region Admin Game Data Endpoints

        [HttpGet]
        public async Task<ActionResult<List<BoardBM>>> Boards()
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await _userAdapter.GetUser(token);
            if (!requestingUser.IsAdmin)
                throw new WebException(HttpStatusCode.Forbidden, "Non-Administrators cannot access Board information");
            List<BoardBE> boardBEs = await _gameAdapter.GetAllBoards();
            List<BoardBM> boardBMs = boardBEs?.Select(board => _mapper.Map<BoardBM>(board)).ToList();
            return boardBMs;
        }

        #endregion
    }
}
