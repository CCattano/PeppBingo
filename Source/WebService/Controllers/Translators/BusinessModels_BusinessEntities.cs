using AutoMapper;
using Pepp.Web.Apps.Bingo.BusinessEntities.User;
using Pepp.Web.Apps.Bingo.BusinessModels.User;
using Pepp.Web.Apps.Bingo.Infrastructure;
using Pepp.Web.Apps.Bingo.WebService.Controllers.Translators.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers.Translators
{
    public class BusinessModels_BusinessEntities : Profile
    {
        public BusinessModels_BusinessEntities()
        {
            this.RegisterTranslator<UserBM, UserBE, UserBM_UserBE>();
        }
    }
}
