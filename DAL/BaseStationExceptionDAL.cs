using System;
using System.Runtime.Serialization;

namespace IDAL
{
    [Serializable]
    internal class BaseStationExceptionDAL : Exception
    {
        public BaseStationExceptionDAL()
        {
        }

        public BaseStationExceptionDAL(string message) : base("DAL - Base Station Exception:" + message)
        {
        }

        public BaseStationExceptionDAL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseStationExceptionDAL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}