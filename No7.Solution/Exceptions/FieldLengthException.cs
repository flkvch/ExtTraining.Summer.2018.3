using System;
using System.Runtime.Serialization;

namespace No7.Solution.Exceptions
{
    [Serializable]
    internal class FieldLengthException : Exception
    {
        public FieldLengthException()
        {
        }

        public FieldLengthException(string message) : base(message)
        {
        }

        public FieldLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FieldLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}