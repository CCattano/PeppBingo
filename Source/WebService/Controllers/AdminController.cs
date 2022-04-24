﻿using AutoMapper;
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
            await ConfirmIsAdmin("Non-Administrators cannot search for users");
            List<UserBE> userBEs = await _userAdapter.GetUsers(name);
            List<UserBM> userBMs = userBEs?.Select(userBE => _mapper.Map<UserBM>(userBE)).ToList();
            return Ok(userBMs);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserBM>>> Admins()
        {
            await ConfirmIsAdmin("Non-Administrators cannot search for users");
            List<UserBE> userBEs = await _userAdapter.GetAdminUsers();
            List<UserBM> userBMs = userBEs?.Select(userBE => _mapper.Map<UserBM>(userBE)).ToList();
            return Ok(userBMs);
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
            await ConfirmIsAdmin("Non-Administrators cannot modify permissions of other users");
            await _userAdapter.SetAdminPermissionForUser(userID, isAdmin);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserBM>>> GetUsersByIDs([FromQuery] List<int> userIDs)
        {
            await ConfirmIsAdmin("Non-Administrators cannot search for users");
            List<UserBE> userBEs = await _userAdapter.GetUsers(userIDs);
            List<UserBM> userBMs = userBEs?.Select(userBE => _mapper.Map<UserBM>(userBE)).ToList();
            return Ok(userBMs);
        }

        #endregion

        #region Admin Game Data Endpoints

        [HttpPost]
        public async Task<ActionResult<BoardBM>> CreateBoard([FromBody] BoardBM newBoard)
        {
            UserBE requestingUser = await ConfirmIsAdmin("Non-Administrators cannot create new Boards");
            BoardBE newBoardBE = _mapper.Map<BoardBE>(newBoard);
            BoardBE boardBE = await _gameAdapter.CreateBoard(requestingUser.UserID, newBoardBE);
            BoardBM boardBM = _mapper.Map<BoardBM>(boardBE);
            return Ok(boardBM);
        }

        [HttpGet]
        public async Task<ActionResult<List<BoardBM>>> Boards()
        {
            await ConfirmIsAdmin("Non-Administrators cannot access Board information");
            List<BoardBE> boardBEs = await _gameAdapter.GetAllBoards();
            List<BoardBM> boardBMs = boardBEs?.Select(board => _mapper.Map<BoardBM>(board)).ToList();
            return Ok(boardBMs);
        }

        [HttpPut]
        public async Task<ActionResult<BoardBM>> UpdateBoard([FromBody] BoardBM board)
        {
            UserBE requestingUser = await ConfirmIsAdmin("Non-Administrators cannot create new Boards");
            BoardBE newBoardBE = _mapper.Map<BoardBE>(board);
            BoardBE boardBE = await _gameAdapter.UpdateBoard(requestingUser.UserID, newBoardBE);
            BoardBM boardBM = _mapper.Map<BoardBM>(boardBE);
            return Ok(boardBM);
        }

        #endregion

        private async Task<UserBE> ConfirmIsAdmin(string rejectionMsg)
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await _userAdapter.GetUser(token);
            if (!requestingUser.IsAdmin)
                throw new WebException(HttpStatusCode.Forbidden, rejectionMsg);
            return requestingUser;
        }
    }
}