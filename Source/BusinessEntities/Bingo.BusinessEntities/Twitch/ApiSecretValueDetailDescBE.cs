using ApiSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants
    .ValueDetails.Api.ApiSecrets.Sources;
using TwitchTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants
    .ValueDetails.Api.ApiSecrets.Types.Twitch.Types;

namespace Pepp.Web.Apps.Bingo.BusinessEntities.Twitch
{
    /// <summary>
    /// A Twitch-specific ValueDetailDesc we are storing in the Db for security purposes
    /// </summary>
    public class ApiSecretValueDetailDescBE
    {
        /// <summary>
        /// The source this value detail is associated with
        /// </summary>
        public ApiSources Source;
        /// <summary>
        /// The type of value detail this is.
        /// </summary>
        /// <remarks>
        /// The value of this property is contextual to the source
        /// </remarks>
        public TwitchTypes Type;
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
