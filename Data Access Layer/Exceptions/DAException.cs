using System;

namespace Data_Access_Layer.Exceptions
{
    public class DAException : Exception
    {
        public DAException(string message) : base(message)
        {
        }
        public DAException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
