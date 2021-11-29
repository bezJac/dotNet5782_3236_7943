using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// Exception for invalid attempt to add new  entity to list
    /// </summary>
    [Serializable]
    internal class AddException : Exception
    {
        readonly static string  info = "AddException() ";
        public AddException() { }
        public AddException(string message) : base(info + message) { }
        public AddException(string message, Exception inner) : base(info + message, inner) { }
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
        readonly static string info = "ActionException() ";
        public ActionException() { }
        public ActionException(string message) : base(info + message) { }
        public ActionException(string message, Exception inner) : base(info + message, inner) { }
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
        readonly static string info = "UpdateException() ";
        public UpdateException() { }
        public UpdateException(string message) : base(info + message) { }
        public UpdateException(string message, Exception inner) : base(info + message, inner) { }
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
        readonly static string info = "GetInstanceException() ";
        public GetInstanceException() { }
        public GetInstanceException(string message) : base(info+message) { }
        public GetInstanceException(string message, Exception inner) : base(info+message, inner) { }
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
        readonly static string info = "GetListException() ";
        public GetListException() { }
        public GetListException(string message) : base(info+message) { }
        public GetListException(string message, Exception inner) : base(info+message, inner) { }
        protected GetListException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

}
