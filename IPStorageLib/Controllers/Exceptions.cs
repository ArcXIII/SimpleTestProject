using System;
using System.Runtime.Serialization;

namespace IPStorageLib.Controllers
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string str) : base(str)
        {
        }

        public NotFoundException(string str, Exception inner) : base(str, inner)
        {
        }

        protected NotFoundException(SerializationInfo sinfo, StreamingContext scon) : base(sinfo, scon)
        {
        }

        public override string ToString()
        {
            return Message;
        }
    }

    public class NotConnectedException : Exception
    {
        public NotConnectedException()
        {
        }

        public NotConnectedException(string str) : base(str)
        {
        }

        public NotConnectedException(string str, Exception inner) : base(str, inner)
        {
        }

        protected NotConnectedException(SerializationInfo sinfo, StreamingContext scon) : base(sinfo, scon)
        {
        }

        public override string ToString()
        {
            return Message;
        }
    }
}