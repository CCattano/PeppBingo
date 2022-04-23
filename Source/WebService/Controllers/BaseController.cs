using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Pepp.Web.Apps.Bingo.Infrastructure;
using Pepp.Web.Apps.Bingo.Managers;
using System;
using System.Linq;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    public abstract class BaseController<TAdapter> : BaseController
    {
        protected readonly TAdapter Adapter;

        protected BaseController(TAdapter adapter) : base() => Adapter = adapter;
    }

    public abstract class BaseController : Controller
    {
        protected string BaseUri => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

        protected string TryGetAccessTokenFromRequestHeader() =>
            HttpContext.Request.Headers.TryGetValue(TokenManager.AccessJWTCookieName, out StringValues headerVal)
                ? headerVal.FirstOrDefault()
                : null;

        protected void SetAuthJWTCookieHeader(string token)
        {
            string cookie = $"{TokenManager.AuthJWTCookieName}={token};max-age={TimeSpan.FromMinutes(1).TotalSeconds};path=/";
            HttpContext.Response.Headers.Add(SystemConstants.Headers.SetCookie, cookie);
        }
    }
}
