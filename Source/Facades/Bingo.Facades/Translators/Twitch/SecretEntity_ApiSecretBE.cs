using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using System;
using ApiSecretSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Sources;
using TwitchSecretTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants.ApiSecrets.Types.Twitch;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Twitch
{
    /// <summary>
    /// Translates a generic ValueDetailDesc Entity to a Twitch-specific ValueDetailDesc
    /// </summary>
    public class SecretEntity_ApiSecretBE : ITypeConverter<SecretEntity, ApiSecretBE>, ITypeConverter<ApiSecretBE, SecretEntity>
    {
        public ApiSecretBE Convert(SecretEntity source, ApiSecretBE destination, ResolutionContext context)
        {
            ApiSecretSources valueDetailDescSource = Enum.Parse<ApiSecretSources>(source.Source);
            if (valueDetailDescSource != ApiSecretSources.Twitch)
                throw new InvalidCastException("Cannot convert a non-Twitch Secret to a Twitch-specific Secret");

            destination ??= new();
            destination.Source = valueDetailDescSource;
            destination.Type = Enum.Parse<TwitchSecretTypes>(source.Type);
            destination.Value = source.Value;
            destination.Description = source.Description;
            return destination;
        }

        public SecretEntity Convert(ApiSecretBE source, SecretEntity destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
