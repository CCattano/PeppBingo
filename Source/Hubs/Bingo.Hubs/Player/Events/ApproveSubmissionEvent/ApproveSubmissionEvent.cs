namespace Pepp.Web.Apps.Bingo.Hubs.Player.Events.ApproveSubmissionEvent
{
    public class ApproveSubmissionEvent
    {
        /// <summary>
        /// The internal UserID of the user who responded to the bingo
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// The display name of the user who responded to the bingo
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// The profile image uri of the use who responded to the bingo
        /// </summary>
        public string ProfileImageUri { get; set; }
    }
}