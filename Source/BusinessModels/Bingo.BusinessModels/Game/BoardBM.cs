using System;

namespace Pepp.Web.Apps.Bingo.BusinessModels.Game
{
    /*
     * The data returned in this Business Model is for ADMIN EYES ONLY
     *
     * If you find yourself trying to return this object to the non-administrator front end
     * think through how necessary it really is for public users to see this information
     *
     * A BoardID alone should be sufficient for all public front end operations.
     * MAYBE including BoardName in the future, but desc, and create/mod info is unnecessary
     */
    public class BoardBM
    {
        /// <summary>
        /// The internal ID that represent this bingo board
        /// </summary>
        public int BoardID;
        /// <summary>
        /// The name of the bingo board
        /// </summary>
        public string Name;
        /// <summary>
        /// A brief dsescription about the bingo board
        /// </summary>
        public string Description;
        /// <summary>
        /// Quantity of active tiles available for this board
        /// </summary>
        public string TileCount;
        /// <summary>
        /// When the board was created
        /// </summary>
        public DateTime CreatedDateTime;
        /// <summary>
        /// The UserID of who created the board
        /// </summary>
        public int CreatedBy;
        /// <summary>
        /// When details about the bingo board were last edited
        /// </summary>
        public DateTime ModDateTime;
        /// <summary>
        /// Who last edited details about the bingo board
        /// </summary>
        public int ModBy;
    }
}
