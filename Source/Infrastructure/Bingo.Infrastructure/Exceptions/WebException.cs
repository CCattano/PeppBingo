using System;
using System.Net;

namespace Pepp.Web.Apps.Bingo.Infrastructure.Exceptions
{
    public class WebException : Exception
    {
        public HttpStatusCode ResponseCode;
        public new string Message;

        public WebException(HttpStatusCode responseCode, string message)
        {
            ResponseCode = responseCode;
            Message = message;
        }
    }
}
