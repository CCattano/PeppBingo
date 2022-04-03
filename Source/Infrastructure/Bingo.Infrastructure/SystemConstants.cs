namespace Pepp.Web.Apps.Bingo.Infrastructure
{
    public struct SystemConstants
    {
        /// <summary>
        /// Constants related to the various ValueDetails 
        /// that can be fetched throughout the application
        /// </summary>
        public struct ValueDetails
        {
            /// <summary>
            /// ValueDetails data in the API schema
            /// </summary>
            public struct Api
            {
                /// <summary>
                /// ApiSecrets Value Details
                /// </summary>
                public struct ApiSecrets
                {
                    /// <summary>
                    /// Available Sources For the ApiSecrets Value Details
                    /// </summary>
                    public enum Sources
                    {
                        Twitch
                    }
                }
            }
        }

        /// <summary>
        /// Constants related to data found in
        /// the AppSettings configuration file
        /// </summary>
        public struct AppSettings
        {
            public struct ConnStrings
            {
                public const string PeppBingo = "PeppBingo";
            }
        }
    }
}
