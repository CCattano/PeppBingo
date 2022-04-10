namespace Pepp.Web.Apps.Bingo.Infrastructure
{
    public struct SystemConstants
    {
        /// <summary>
        /// Constants for the ApiSecrets Value Details
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

            /// <summary>
            /// The ValueDetail Types availble
            /// </summary>
            /// <remarks>
            /// Organized by Type Source
            /// </remarks>
            public struct Types
            {
                /// <summary>
                /// The ValueDetail Types available for Twitch
                /// </summary>
                public enum Twitch
                {
                    /// <summary>
                    /// The ValueDetail contains the ClientID
                    /// </summary>
                    ClientID,
                    /// <summary>
                    /// The ValueDetail contains the ClientSecret
                    /// </summary>
                    ClientSecret
                }
            }
        }

        /// <summary>
        /// Constants related to data found in
        /// the AppSettings configuration file
        /// </summary>
        public struct AppSettings
        {
            /// <summary>
            /// ConnectionStrings found in the
            /// AppSettings configuration file
            /// </summary>
            public struct ConnStrings
            {
                public const string PeppBingo = "PeppBingo";
            }
        }
    }
}
