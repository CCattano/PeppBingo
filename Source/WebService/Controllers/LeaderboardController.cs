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
            
            List<int> boardIDs = leaderboardBEs.Select(leaderboard => leaderboard.BoardID).ToList();
            List<BoardBE> boardBEs = await _gameAdapter.GetBoards(boardIDs);
            
            Dictionary<int, string> boardNamesByBoardID =
                boardBEs.ToDictionary(key => key.BoardID, value => value.Name);
            
            List<LeaderboardBM> leaderboardBMs =
                leaderboardBEs
                    .Select(leaderboardBE => new LeaderboardBM
                    {
                        LeaderboardID = leaderboardBE.LeaderboardID,
                        BoardName = boardNamesByBoardID[leaderboardBE.BoardID]
                    })
                    .ToList();
            return leaderboardBMs;
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