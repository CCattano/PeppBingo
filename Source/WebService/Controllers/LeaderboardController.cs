﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Adapters;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.Hubs.Player;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission;
using Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [TokenRequired]
    [Route("[controller]/[action]")]
    public class LeaderboardController : BaseController
    {
        private readonly IPlayerHub _playerHub;
        private readonly IUserAdapter _userAdapter;

        public LeaderboardController(IPlayerHub playerHub, IUserAdapter userAdapter)
        {
            _playerHub = playerHub;
            _userAdapter = userAdapter;
        }

        [HttpPost]
        public async Task<ActionResult> BingoSubmission([FromBody] BingoSubmissionEvent submission)
        {
            UserBE requestingUser = await GetRequestingUser();
            submission.UserID = requestingUser.UserID;
            await _playerHub.EmitBingoSubmission(submission);
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