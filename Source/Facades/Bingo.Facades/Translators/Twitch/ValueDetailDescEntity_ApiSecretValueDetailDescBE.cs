using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using System;
using ApiSources =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants
    .ValueDetails.Api.ApiSecrets.Sources;
using TwitchTypes =
    Pepp.Web.Apps.Bingo.Infrastructure.SystemConstants
    .ValueDetails.Api.ApiSecrets.Types.Twitch.Types;

namespace Pepp.Web.Apps.Bingo.Facades.Translators.Twitch
{
    /// <summary>
    /// Translates a generic ValueDetailDesc Entity to a Twitch-specific ValueDetailDesc
    /// </summary>
    public class ValueDetailDescEntity_ApiSecretValueDetailDescBE : ITypeConverter<ValueDetailDescEntity, ApiSecretValueDetailDescBE>, ITypeConverter<ApiSecretValueDetailDescBE, ValueDetailDescEntity>
    {
        public ApiSecretValueDetailDescBE Convert(ValueDetailDescEntity source, ApiSecretValueDetailDescBE destination, ResolutionContext context)
        {
            ApiSources valueDetailDescSource = Enum.Parse<ApiSources>(source.Source);
            if (valueDetailDescSource != ApiSources.Twitch)
                throw new InvalidCastException("Cannot convert a non-Twitch ValueDetail to a Twitch-specific ValueDetail");

            destination ??= new ApiSecretValueDetailDescBE();
            destination.Source = valueDetailDescSource;
            destination.Type = Enum.Parse<TwitchTypes>(source.Type);
            destination.Value = source.Value;
            destination.Description = source.Description;
            return destination;
        }

        public ValueDetailDescEntity Convert(ApiSecretValueDetailDescBE source, ValueDetailDescEntity destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
