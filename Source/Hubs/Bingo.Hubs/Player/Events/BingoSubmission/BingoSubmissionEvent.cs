using System.Collections.Generic;

namespace Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission
{
    public class BingoSubmissionEvent
    {
        /// <summary>
        /// The SignalR Hub Connection ID for this user.
        /// Used so that the Vote event is not sent to the user requesting the vote
        /// </summary>
        public string SubmitterConnectionID;
        /// <summary>
        /// The internal UserID of the user
        /// Used so the user cannot have multiple tabs open and vote for themselves
        /// </summary>
        public int UserID;
        /// <summary>
        /// The board tiles that compose the vote requestor's board at the time of bingo submission
        /// </summary>
        public List<TileDetail> BoardTiles;
    }
}
