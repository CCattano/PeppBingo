using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.Game;
using Pepp.Web.Apps.Bingo.BusinessEntities.Stats;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.Stats;
using Pepp.Web.Apps.Bingo.Hubs.Player;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.ApproveSubmissionEvent;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [TokenRequired]
    [Route("[controller]/[action]")]
    public class LeaderboardController : BaseController
    {
        private readonly IPlayerHub _playerHub;
        private readonly IStatsAdapter _statsAdapter;
        private readonly IGameAdapter _gameAdapter;
        private readonly IUserAdapter _userAdapter;

        public LeaderboardController(
            IPlayerHub playerHub,
            IStatsAdapter statsAdapter,
            IGameAdapter gameAdapter,
            IUserAdapter userAdapter
        )
        {
            _playerHub = playerHub;
            _statsAdapter = statsAdapter;
            _gameAdapter = gameAdapter;
            _userAdapter = userAdapter;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaderboardBM>>> All()
        {
            List<LeaderboardBE> leaderboardBEs = await _statsAdapter.GetAllLeaderboards();

            if (leaderboardBEs == null) return NoContent();

            List<int> boardIDs = leaderboardBEs.Select(leaderboard => leaderboard.BoardID).ToList();
            List<BoardBE> boardBEs = await _gameAdapter.GetBoards(boardIDs);
            boardBEs = boardBEs.Where(b => b.TileCount >= 25).ToList();

            Dictionary<int, LeaderboardBE> leaderboardsByBoardID =
                leaderboardBEs.ToDictionary(key => key.BoardID);

            List<LeaderboardBM> leaderboardBMs = boardBEs.Select(boardBE => new LeaderboardBM
            {
                LeaderboardID = leaderboardsByBoardID[boardBE.BoardID].LeaderboardID,
                BoardName = boardBE.Name
            }).ToList();
            return leaderboardBMs;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaderboardPosBM>>> Positions([FromQuery] int leaderboardID)
        {
            List<LeaderboardPosBE> positions =
                await _statsAdapter.GetLeaderboardPositions(leaderboardID);

            if (positions == null) return NoContent();

            Dictionary<int, LeaderboardPosBE> positionsByUserID = positions.ToDictionary(k => k.UserID);

            List<int> userIDs = positions.Select(p => p.UserID).Distinct().ToList();
            List<UserBE> users = await _userAdapter.GetUsers(userIDs);

            List<LeaderboardPosBM> result = users.Select(u => new LeaderboardPosBM()
            {
                LeaderboardID = leaderboardID,
                DisplayName = u.DisplayName,
                ProfileImageUri = u.ProfileImageUri,
                BingoQty = positionsByUserID[u.UserID].BingoQty
            }).ToList();

            return result;
        }

        [HttpPut]
        public async Task UpdatePosition([FromQuery] int boardID)
        {
            UserBE requestingUser = await GetRequestingUser();
            await _statsAdapter.UpdatePosition(requestingUser.UserID, boardID);
        }

        [HttpPost]
        public async Task<ActionResult> BingoSubmission([FromBody] BingoSubmissionEvent submission)
        {
            UserBE requestingUser = await GetRequestingUser();
            submission.UserID = requestingUser.UserID;
            await _playerHub.EmitBingoSubmission(submission);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CancelSubmission([FromQuery] string hubConnID)
        {
            await _playerHub.EmitSubmissionCancel(hubConnID);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> ApproveSubmission([FromQuery] string requestorHubConnID)
        {
            UserBE requestingUser = await GetRequestingUser();
            ApproveSubmissionEvent evtData = new()
            {
                UserID = requestingUser.UserID,
                DisplayName = requestingUser.DisplayName,
                ProfileImageUri = requestingUser.ProfileImageUri
            };
            await _playerHub.EmitApproveSubmission(requestorHubConnID, evtData);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> RejectSubmission([FromQuery] string requestorHubConnID)
        {
            await _playerHub.EmitRejectSubmission(requestorHubConnID);
            return Ok();
        }

        private async Task<UserBE> GetRequestingUser()
        {
            string token = base.TryGetAccessTokenFromRequestHeader();
            UserBE requestingUser = await _userAdapter.GetUser(token);
            return requestingUser;
        }
    }
}