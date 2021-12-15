using System;
using System.Runtime.Serialization;
using DO;
using DalApi;
using DS;

namespace Dal
{
    /// <summary>
    /// Exception if entity with specific id already exists in list
    /// </summary>
    [Serializable]
    internal class ExsistException : Exception
    {
        readonly private static string info = "DAL - ExsistException(): ";
        public ExsistException()
        {
        }

        public ExsistException(string message) : base(  info + message)
        {
        }

        public ExsistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExsistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}