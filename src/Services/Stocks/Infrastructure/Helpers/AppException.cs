using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class AppException : Exception
    {
        public AppException() : base() {}

        public AppException(string message) : base(message)
        {
            throw new HttpStatusException((int)HttpStatusCode.InternalServerError, message);
        }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}