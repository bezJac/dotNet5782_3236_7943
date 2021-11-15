using System;
using System.Runtime.Serialization;

namespace BL.BO
{
    [Serializable]
    internal class DroneException : Exception
    {
        static string info = "Drone Exception: ";
        public DroneException()
        {
        }

        public DroneException(string message) : base(info + message)
        {
        }

        public DroneException(string message, Exception innerException) : base(message+info, innerException)
        {
        }

        protected DroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}