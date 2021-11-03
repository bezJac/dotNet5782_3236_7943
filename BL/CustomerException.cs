using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class CustomerException : Exception
    {
        static string info = "Customer Exception: ";
        public CustomerException()
        {
        }

        public CustomerException(string message) : base(info + message)
        {
        }

        public CustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}