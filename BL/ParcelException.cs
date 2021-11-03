using System;
using System.Runtime.Serialization;

namespace BL.BO
{
    [Serializable]
    internal class ParcelException : Exception
    {
        static string info = "Parcel Exception: ";
        public ParcelException()
        {
        }

        public ParcelException(string message) : base(info + message)
        {
        }

        public ParcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}