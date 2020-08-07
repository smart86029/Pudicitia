using System;

namespace Pudicitia.Common.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(string message) : base(message)
        {
        }
    }
}