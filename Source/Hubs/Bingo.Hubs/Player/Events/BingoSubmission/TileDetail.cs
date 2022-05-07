namespace Pepp.Web.Apps.Bingo.Hubs.Player.Events.BingoSubmission
{
    public class TileDetail
    {
        /// <summary>
        /// The text of the bingo tile
        /// </summary>
        public string Text;
        /// <summary>
        /// Bool flag indicating if tile is selected
        /// </summary>
        public bool IsSelected;
        /// <summary>
        /// The row the bingo tile is associated with
        /// </summary>
        public int Row;
        /// <summary>
        /// The column the bingo tile is associated with
        /// </summary>
        public int Column;
    }
}
