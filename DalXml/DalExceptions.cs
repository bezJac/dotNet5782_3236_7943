using System;
using System.Runtime.Serialization;
using DO;
using DalApi;


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

        public ExsistException(string message) : base(info + message)
        {
        }

        public ExsistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExsistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
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

        public FilteredListException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FilteredListException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
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

        public NonExistsException(string message) : base(message)
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