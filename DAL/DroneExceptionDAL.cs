using System;
using System.Runtime.Serialization;

namespace IDAL
{
    [Serializable]
    public class DroneExceptionDAL : Exception
    {
        public DroneExceptionDAL()
        {
        }

        public DroneExceptionDAL(string message) : base("DAL: Drone Exception: " + message)
        {
        }

        public DroneExceptionDAL(string message, Exception innerException) : base( message, innerException)
        {
        }

        protected DroneExceptionDAL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}