using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Pepp.Web.Apps.Bingo.Managers;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenManager tokenMan)
        {
            //Get Token from header
            if (!context.Request.Headers.TryGetValue(TokenManager.AccessJWTCookieName, out StringValues headerVal))
                throw new WebException(HttpStatusCode.BadRequest, "A JWT was required for the request made and could not be found");

            string token = headerVal.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(token))
                throw new WebException(HttpStatusCode.BadRequest, "A JWT was required for the request made and could not be found");

            // ---Validations---
            // Signature validation
            bool isValid = tokenMan.TokenIsValid(token);
            if (!isValid)
                throw new WebException(HttpStatusCode.BadRequest, "The JWT provided was not valid");
            
            // Expired validation
            isValid = !TokenManager.TokenIsExpired(token);
            if (!isValid && context.Request.Path != "/Twitch/RefreshToken")
                throw new WebException(HttpStatusCode.BadRequest, "The JWT provided was not valid");

            //Token valid, proceed
            await _next(context);
        }
    }
}