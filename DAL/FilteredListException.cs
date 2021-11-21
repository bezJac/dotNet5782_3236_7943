using System;
using System.Runtime.Serialization;

namespace IDAL
{
    /// <summary>
    /// Exception if filtered by condition list is  returned empty
    /// </summary>
    [Serializable]
   internal class FilteredListException : Exception
    {
        public FilteredListException()
        {
        }

        public FilteredListException(string message) : base("DAL: Drone Exception: " + message)
        {
        }

        public FilteredListException(string message, Exception innerException) : base( message, innerException)
        {
        }

        protected FilteredListException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}