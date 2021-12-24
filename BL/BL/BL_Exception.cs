using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Exception for invalid attempt to add new  entity to list
    /// </summary>
    [Serializable]
    internal class AddException : Exception
    {
        public AddException() { }
        public AddException(string message) : base(message) { }
        public AddException(string message, Exception inner) : base(message, inner) { }
        protected AddException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception for invalid attempt to execute an action with a drone
    /// </summary>
    [Serializable]
    internal class ActionException : Exception
    {
        public ActionException() { }
        public ActionException(string message) : base(message) { }
        public ActionException(string message, Exception inner) : base(message, inner) { }
        protected ActionException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception for invalid attempt to update an entities details
    /// </summary>
    [Serializable]
    internal class UpdateException : Exception
    {
        public UpdateException() { }
        public UpdateException(string message) : base(message) { }
        public UpdateException(string message, Exception inner) : base(message, inner) { }
        protected UpdateException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception for invalid attempt to get a single entity
    /// </summary>
    [Serializable]
    internal class GetInstanceException : Exception
    {
        public GetInstanceException() { }
        public GetInstanceException(string message) : base(message) { }
        public GetInstanceException(string message, Exception inner) : base(message, inner) { }
        protected GetInstanceException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception for invalid attempt to get a list of  entities
    /// </summary>
    [Serializable]
    internal class GetListException : Exception
    {
        public GetListException() { }
        public GetListException(string message) : base(message) { }
        public GetListException(string message, Exception inner) : base(message, inner) { }
        protected GetListException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Exception for invalid attempt to remove an entity from list
    /// </summary>
    [Serializable]
    internal class RemoveException : Exception
    {
        public RemoveException() { }
        public RemoveException(string message) : base(message) { }
        public RemoveException(string message, Exception inner) : base(message, inner) { }
        protected RemoveException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

}
