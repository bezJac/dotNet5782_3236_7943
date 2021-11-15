using System;
using System.Runtime.Serialization;

namespace BL
{
    namespace BO
    {
        [Serializable]
        internal class BaseStationException : Exception
        {
            static string info = "Base Station Exception: ";
            public BaseStationException()
            {
            }

            public BaseStationException(string message) : base(info + message)
            {
            }

            public BaseStationException(string message, Exception innerException) : base(message+info, innerException)
            {
            }

            protected BaseStationException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}