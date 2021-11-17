using System;
using System.Runtime.Serialization;

namespace IDAL
{
    [Serializable]
    internal class CustomerExceptionDAL : Exception
    {
        public CustomerExceptionDAL()
        {
        }

        public CustomerExceptionDAL(string message) : base("DAL - Customer Exception: " + message)
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