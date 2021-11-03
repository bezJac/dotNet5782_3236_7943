using System;
using System.Runtime.Serialization;

namespace IDAL
{
    [Serializable]
    public class BaseStationException : Exception
    {
        public BaseStationException()
        {
        }

        public BaseStationException(string message) : base("Base Station Exception:" + message)
        {
        }

        public BaseStationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseStationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}