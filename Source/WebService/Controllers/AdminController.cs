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
        private readonly IStatsAdapter _statsAdapter;

        public AdminController
        (
            IMapper mapper,
            IUserAdapter userAdapter,
            IGameAdapter gameAdapter,
            IStatsAdapter statsAdapter
        )
        {
            _mapper = mapper;
            _userAdapter = userAdapter;
            _gameAdapter = gameAdapter;
            _statsAdapter = statsAdapter;
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
            UserBE requestingUser = await ConfirmIsAdmin("Non-Administrators cannot modify Boards");
            BoardBE boardBE = _mapper.Map<BoardBE>(board);
            BoardBE updatedBoardBE = await _gameAdapter.UpdateBoard(requestingUser.UserID, boardBE);
            BoardBM boardBM = _mapper.Map<BoardBM>(updatedBoardBE);
            return Ok(boardBM);
        }

        [HttpDelete]
        public async Task DeleteBoard([FromQuery] int boardID)
        {
            await ConfirmIsAdmin("Non-Administrators cannot delete Boards");
            await _gameAdapter.DeleteBoard(boardID);
        }

        [HttpPost]
        public async Task<ActionResult<BoardTileBM>> CreateBoardTile([FromQuery] int boardID, [FromBody] BoardTileBM newTile)
        {
            UserBE requestingUser = await ConfirmIsAdmin("Non-Administrators cannot create new Board Tiles");
            BoardTileBE newBoardTileBE = _mapper.Map<BoardTileBE>(newTile);
            BoardTileBE boardTileBE =
                await _gameAdapter.CreateBoardTile(requestingUser.UserID, boardID, newBoardTileBE);
            BoardTileBM boardTileBM = _mapper.Map<BoardTileBM>(boardTileBE);
            return boardTileBM;
        }

        [HttpGet]
        public async Task<ActionResult<List<BoardTileBM>>> Tiles([FromQuery] int boardID)
        {
            await ConfirmIsAdmin("Non-Administrators cannot access Board information");
            List<BoardTileBE> boardTileBEs = await _gameAdapter.GetAllBoardTiles(boardID);
            List<BoardTileBM> boardTileBMs = boardTileBEs?.Select(tile => _mapper.Map<BoardTileBM>(tile)).ToList();
            return Ok(boardTileBMs);
        }

        [HttpPut]
        public async Task<ActionResult<BoardTileBM>> UpdateBoardTile([FromBody] BoardTileBM boardTile)
        {
            UserBE requestingUser = await ConfirmIsAdmin("Non-Administrators cannot modify Board Tiles");
            BoardTileBE boardTileBE = _mapper.Map<BoardTileBE>(boardTile);
            BoardTileBE updatedBoardTileBE = await _gameAdapter.UpdateBoardTile(requestingUser.UserID, boardTileBE);
            BoardTileBM boardTileBM = _mapper.Map<BoardTileBM>(updatedBoardTileBE);
            return Ok(boardTileBM);
        }

        [HttpDelete]
        public async Task DeleteBoardTile([FromQuery] int tileID)
        {
            await ConfirmIsAdmin("Non-Administrators cannot delete Board Tiles");
            await _gameAdapter.DeleteBoardTile(tileID);
        }

        #endregion

        #region Admin Leaderboard Data Endpoints

        [HttpPut]
        public async Task<ActionResult> ResetLeaderboard([FromQuery]int leaderboardID)
        {
            await ConfirmIsAdmin("Non-Administrators cannot modify Leaderboards");
            await _statsAdapter.ResetLeaderboard(leaderboardID);
            return Ok();
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