using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission;

namespace Pepp.Web.Apps.Bingo.Hubs.Player
{
    public interface IPlayerHub
    {
        /// <summary>
        /// Emits an event containing the ID of the
        /// new board all players should be playing
        /// </summary>
        /// <returns></returns>
        Task EmitLatestActiveBoardID(int activeBoardID);

        /// <summary>
        /// Emits an event containing the bingo board data
        /// of a user who has achieved a bingo
        /// </summary>
        /// <param name="submission"></param>
        /// <returns></returns>
        Task EmitBingoSubmission(BingoSubmissionEvent submission);

        /// <summary>
        /// Emits a void event to all connected clients that a user who
        /// submitted a bingo for voting has canceled their request
        /// </summary>
        /// <param name="hubConnID"></param>
        /// <returns></returns>
        Task EmitSubmissionCancel(string hubConnID);
    }

    public class PlayerHub : IPlayerHub
    {
        private readonly IHubContext<BasePlayerHub, IBasePlayerHub> _playerHub;

        public PlayerHub(IHubContext<BasePlayerHub, IBasePlayerHub> playerHub) =>
            _playerHub = playerHub;

        public async Task EmitLatestActiveBoardID(int activeBoardID) =>
            await _playerHub.Clients.All.LatestActiveBoardID(activeBoardID);

        public async Task EmitBingoSubmission(BingoSubmissionEvent submission) =>
            await _playerHub.Clients.AllExcept(submission.SubmitterConnectionID).BingoSubmission(submission);

        public async Task EmitSubmissionCancel(string hubConnID) =>
            await _playerHub.Clients.AllExcept(hubConnID).CancelSubmission(hubConnID);
    }
}