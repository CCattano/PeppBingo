using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Hubs.Player;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class LeaderboardController : BaseController
    {
        private readonly IPlayerHub _playerHub;

        public LeaderboardController(IPlayerHub playerHub) =>
            _playerHub = playerHub;

        [HttpPost]
        public async Task<ActionResult> BingoSubmission([FromBody] BingoSubmissionEvent submission)
        {
            await _playerHub.EmitBingoSubmission(submission);
            return Ok();
        }
    }
}