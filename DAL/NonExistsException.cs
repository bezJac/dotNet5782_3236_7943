using System;
using System.Runtime.Serialization;

namespace IDAL
{
    /// <summary>
    /// Exception if entity with requested id does not exsist in list
    /// </summary>
    [Serializable]
    internal class NonExistsException : Exception
    {
        private static string info = "DAL - NonExistsException(): ";
        public NonExistsException()
        {
        }

        public NonExistsException(string message) : base( info + message)
        {
        }

        public NonExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NonExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}