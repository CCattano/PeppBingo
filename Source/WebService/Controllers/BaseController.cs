using Microsoft.AspNetCore.Mvc;
using Pepp.Web.Apps.Bingo.Infrastructure;
using System;

namespace Pepp.Web.Apps.Bingo.WebService.Controllers
{
    public abstract class BaseController<TAdapter> : Controller
    {
        protected readonly TAdapter Adapter;
        protected string BaseUri => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        
        public BaseController(TAdapter adapter) => Adapter = adapter;

        protected void SetJWTCookieHeader(string token)
        {
            string cookie = $"{SystemConstants.Headers.JWTCookieName}={token};max-age={TimeSpan.FromMinutes(1).TotalSeconds};path=/";
            HttpContext.Response.Headers.Add(SystemConstants.Headers.SetCookie, cookie);
        }
    }
}
