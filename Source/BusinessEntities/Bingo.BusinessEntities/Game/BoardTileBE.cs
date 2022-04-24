using System;

namespace Pepp.Web.Apps.Bingo.BusinessEntities.Game
{
    public class BoardTileBE
    {
        /// <summary>
        /// The internal ID that represent this bingo board tile
        /// </summary>
        public int TileID;
        /// <summary>
        /// The internal ID of the bingo board this tile belongs to
        /// </summary>
        public int BoardID;
        /// <summary>
        /// The text content of this bingo board tile to be displayed on the board
        /// </summary>
        public string Text;
        /// <summary>
        /// Flag indicating if this tile is enabled for use in bingo games
        /// </summary>
        public bool IsActive;
        /// <summary>
        /// When the board tile was created
        /// </summary>
        public DateTime CreatedDateTime;
        /// <summary>
        /// The UserID of who created the board tile
        /// </summary>
        public int CreatedBy;
        /// <summary>
        /// When details about the bingo board tile were last edited
        /// </summary>
        public DateTime ModDateTime;
        /// <summary>
        /// Who last edited details about the bingo board tile
        /// </summary>
        public int ModBy;
    }
}
