using System;
using System.Runtime.Serialization;

namespace No7.Solution.Exceptions
{
    [Serializable]
    internal class TradePriceException : Exception
    {
        public TradePriceException()
        {
        }

        public TradePriceException(string message) : base(message)
        {
        }

        public TradePriceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TradePriceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}