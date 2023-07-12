using System;

namespace Business_Logic_Layer.Exceptions
{
    public class DataValidException : Exception
    {
        public DataValidException(string message) : base(message)
        {
        }
        public DataValidException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
