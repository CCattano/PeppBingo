using AutoMapper;
using Pepp.Web.Apps.Bingo.Data.Entities.Common;
using Pepp.Web.Apps.Bingo.Infrastructure;
using TwitchBE = Pepp.Web.Apps.Bingo.BusinessEntities.Twitch;
using TwitchTranslators = Pepp.Web.Apps.Bingo.Facades.Translators.Twitch;

namespace Pepp.Web.Apps.Bingo.Facades.Translators
{
    public class Entity_BusinessEntity : Profile
    {
        public Entity_BusinessEntity()
        {
            this.RegisterTranslator<ValueDetailDescEntity, TwitchBE.ApiSecretValueDetailDescBE, TwitchTranslators.ValueDetailDescEntity_ApiSecretValueDetailDescBE>();
        }
    }
}
