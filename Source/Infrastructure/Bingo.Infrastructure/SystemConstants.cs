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
        /// Constants for the TokenSecrets Value Details
        /// </summary>
        public struct TokenSecrets
        {
            /// <summary>
            /// Available Sources For the TokenSecrets Value Details
            /// </summary>
            public enum Sources
            {
                JWT
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
                /// The TokenSecret Types available for JWT
                /// </summary>
                public enum JWT
                {
                    /// <summary>
                    /// The ValueDetail contains the SHA256Key
                    /// </summary>
                    SHA256Key,
                    /// <summary>
                    /// The ValueDetail contains the SigningSecret
                    /// </summary>
                    SigningSecret
                }
            }
        }

        /// <summary>
        /// Constant values related to response 
        /// header data set by the server
        /// </summary>
        public struct Headers
        {
            public const string SetCookie = "set-cookie";
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
