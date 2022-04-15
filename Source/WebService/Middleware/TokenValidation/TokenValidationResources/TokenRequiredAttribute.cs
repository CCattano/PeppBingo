using System;
using System.Runtime.CompilerServices;

namespace Pepp.Web.Apps.Bingo.WebService.Middleware.TokenValidation.TokenValidationResources
{
    /// <summary>
    /// Attribute placed over endpoints that require a JWT in the header of the request
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         TokenValidationMiddleware will be registered with
    ///         the IAppBuilder to only use middleware for endpoints 
    ///         decorated with the TokenRequired attribute
    ///     </para>
    /// </remarks>
    public class TokenRequired : Attribute
    {
        public readonly string EndpointName;
        public TokenRequired([CallerMemberName] string endpointName = null) => EndpointName = endpointName;
    }
}
