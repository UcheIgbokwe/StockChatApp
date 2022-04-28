using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid Credentials. Please try again.")
        { }
        public InvalidCredentialsException(string message) : base(message)
        {
            throw new HttpStatusException((int)HttpStatusCode.BadRequest, message);
        }
        public InvalidCredentialsException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}