using System;
using System.Runtime.Serialization;

namespace IDAL
{
    [Serializable]
    public class ParcelExceptionDAL : Exception
    {
        public ParcelExceptionDAL()
        {
        }

        public ParcelExceptionDAL(string message) : base("DAL - Parcel Exception: " +message)
        {
        }

        public ParcelExceptionDAL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParcelExceptionDAL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}