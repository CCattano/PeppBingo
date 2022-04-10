using ApiSecretSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Sources;
using TwitchSecretTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Types.Twitch;

namespace Pepp.Web.Apps.Bingo.BusinessEntities.Twitch
{
    /// <summary>
    /// A Twitch-specific ValueDetailDesc we are storing in the Db for security purposes
    /// </summary>
    public class ApiSecretBE
    {
        /// <summary>
        /// The source this value detail is associated with
        /// </summary>
        public ApiSecretSources Source;
        /// <summary>
        /// The type of value detail this is.
        /// </summary>
        /// <remarks>
        /// The value of this property is contextual to the source
        /// </remarks>
        public TwitchSecretTypes Type;
        /// <summary>
        /// The value of this value detail
        /// </summary>
        public string Value;
        /// <summary>
        /// The description of this value detail
        /// </summary>
        public string Description;
    }
}
