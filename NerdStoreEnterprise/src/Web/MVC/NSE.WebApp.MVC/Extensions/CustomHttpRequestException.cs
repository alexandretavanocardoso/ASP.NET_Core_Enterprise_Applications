using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class CustomHttpRequestException : Exception
    {
        public HttpStatusCode _statusCode;

        // Sem passar nada
        public CustomHttpRequestException() { }

        // Passando a mensagem real
        public CustomHttpRequestException(string message, Exception exception) : base(message, exception) { }

        // Criando mensagem personalizada (mais usado)
        public CustomHttpRequestException(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }
    }
}
