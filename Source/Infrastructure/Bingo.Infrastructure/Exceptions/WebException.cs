using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
