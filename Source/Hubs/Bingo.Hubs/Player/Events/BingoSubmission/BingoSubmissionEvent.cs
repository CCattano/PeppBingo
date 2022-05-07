using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission
{
    public class BingoSubmissionEvent
    {
        /// <summary>
        /// The SignalR Hub Connection ID for this user.
        /// Used so that the Vote event is not sent to the user requesting the vote
        /// </summary>
        public string SubmitterConnectionID { get; set; }

        /// <summary>
        /// The internal UserID of the user
        /// Used so the user cannot have multiple tabs open and vote for themselves
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The board tiles that compose the vote requestor's board at the time of bingo submission
        /// </summary>
        public List<TileDetail> BoardTiles { get; set; }
    }

    public class TileDetail
    {
        /// <summary>
        /// The text of the bingo tile
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Bool flag indicating if tile is selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// The row the bingo tile is associated with
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// The column the bingo tile is associated with
        /// </summary>
        public int Column { get; set; }
    }
}