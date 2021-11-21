using System;
using System.Runtime.Serialization;

namespace IDAL
{
  
    [Serializable]
    /// <summary>
    /// Exception if list of data is returned empty
    /// </summary>
    internal class EmptyListException : Exception
    {
        private static string info = "DAL - EmptyListException():  ";
        public EmptyListException()
        {
        }

        public EmptyListException(string message) : base(info + message)
        {
        }

        public EmptyListException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyListException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}