using System;
using System.Runtime.Serialization;

namespace IDAL
{
    [Serializable]
    public class CustomerExceptionDAL : Exception
    {
        public CustomerExceptionDAL()
        {
        }

        public CustomerExceptionDAL(string message) : base("Customer Exception: " + message)
        {
        }

        public CustomerExceptionDAL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerExceptionDAL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}