using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.Api.Exceptions
{
    public class ValidationException : Exception
    {
        public string Code { get; private set; }

        public ValidationException(string message) : base(message)
        {
            Code = "N/A";
        }

        public ValidationException(string code, string message) : base(message)
        {
            Code = code;
        }

        public ValidationException(string code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}
