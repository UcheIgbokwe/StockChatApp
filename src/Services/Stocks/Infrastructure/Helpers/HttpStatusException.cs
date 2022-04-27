using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class HttpStatusException : Exception
    {
        public int StatusCode { get; }

        public HttpStatusException(int statusCode, string msg) : base(msg)
        {
            StatusCode = statusCode;
        }

        public HttpStatusException() : base()
        {
        }

        public HttpStatusException(string message) : base(message)
        {
        }

        public HttpStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}