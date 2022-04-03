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

                    /// <summary>
                    /// The ValueDetail Types availble
                    /// </summary>
                    /// <remarks>
                    /// Organized by implementing Type
                    /// </remarks>
                    public struct Types
                    {
                        /// <summary>
                        /// The ValueDetail Types available for Twitch
                        /// </summary>
                        public struct Twitch
                        {
                            /// <summary>
                            /// The enum of Twitch ValueDetail Types
                            /// </summary>
                            public enum Types
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
