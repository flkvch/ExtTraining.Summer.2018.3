using System;
using System.Runtime.Serialization;

namespace No7.Solution.Console
{
    [Serializable]
    internal class CurrencyException : Exception
    {
        public CurrencyException()
        {
        }

        public CurrencyException(string message) : base(message)
        {
        }

        public CurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}