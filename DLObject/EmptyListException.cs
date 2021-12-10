using System;
using System.Runtime.Serialization;
using DO;
using DalApi;
using DS;

namespace Dal
{
  
    [Serializable]
    /// <summary>
    /// Exception if list of data is returned empty
    /// </summary>
    internal class EmptyListException : Exception
    {
        private static readonly string info = "DAL - EmptyListException():  ";
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