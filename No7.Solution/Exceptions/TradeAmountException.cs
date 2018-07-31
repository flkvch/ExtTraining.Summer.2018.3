using System;
using System.Runtime.Serialization;

namespace No7.Solution.Console
{
    [Serializable]
    internal class TradeAmountException : Exception
    {
        public TradeAmountException()
        {
        }

        public TradeAmountException(string message) : base(message)
        {
        }

        public TradeAmountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TradeAmountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}