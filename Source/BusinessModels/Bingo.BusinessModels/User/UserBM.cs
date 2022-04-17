namespace Pepp.Web.Apps.Bingo.BusinessModels.User
{
    public class UserBM
    {
        /// <summary>
        /// Twitch-provided display name for a User
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Uri to fetch a User's Twitch profile image
        /// </summary>
        public string ProfileImageUri { get; set; }
        /// <summary>
        /// The user is an application Administrator
        /// </summary>
        public bool IsAdmin;
    }
}
