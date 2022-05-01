namespace Pepp.Web.Apps.Bingo.BusinessModels.Game
{
    /// <summary>
    /// A sub-set of information available on a <see cref="BoardTileBM"/>
    /// to be used by non administrative users for gameplay purposes
    /// </summary>
    public class GameTileBM
    {
        /// <summary>
        /// The text content of this bingo board tile to be displayed on the board
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Flag indicating if this tile is to be used as the center tile on the bingo board
        /// </summary>
        public bool IsFreeSpace { get; set; }
    }
}
