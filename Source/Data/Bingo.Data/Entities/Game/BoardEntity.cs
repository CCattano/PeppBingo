using System;

namespace Pepp.Web.Apps.Bingo.Data.Entities.Game
{
    public class BoardEntity
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
