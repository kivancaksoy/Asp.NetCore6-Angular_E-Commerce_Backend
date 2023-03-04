using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBE.Application.Exceptions
{
    public class AuthencticationErrorException : Exception
    {
        public AuthencticationErrorException() : base("Kimlik doğrulama hatası...")
        {
        }

        public AuthencticationErrorException(string? message) : base(message)
        {
        }

        public AuthencticationErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
