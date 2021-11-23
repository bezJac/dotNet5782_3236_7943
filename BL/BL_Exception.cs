using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{

    [Serializable]
    internal class AddException : Exception
    {
        static string info = "AddException() ";
        public AddException() { }
        public AddException(string message) : base(info + message) { }
        public AddException(string message, Exception inner) : base(info + message, inner) { }
        protected AddException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    internal class ActionException : Exception
    {
        static string info = "ActionException() ";
        public ActionException() { }
        public ActionException(string message) : base(info + message) { }
        public ActionException(string message, Exception inner) : base(info + message, inner) { }
        protected ActionException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    internal class UpdateException : Exception
    {
        static string info = "UpdateException() ";
        public UpdateException() { }
        public UpdateException(string message) : base(info + message) { }
        public UpdateException(string message, Exception inner) : base(info + message, inner) { }
        protected UpdateException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    internal class GetInstanceException : Exception
    {
        static string info = "GetInstanceException() ";
        public GetInstanceException() { }
        public GetInstanceException(string message) : base(info+message) { }
        public GetInstanceException(string message, Exception inner) : base(info+message, inner) { }
        protected GetInstanceException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    internal class GetListException : Exception
    {
        static string info = "GetListException() ";
        public GetListException() { }
        public GetListException(string message) : base(info+message) { }
        public GetListException(string message, Exception inner) : base(info+message, inner) { }
        protected GetListException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }

}
