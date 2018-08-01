using No7.Solution;
using No7.Solution.Exceptions;
using No7.Solution.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution
{
    internal class Validator : IValidator
    {
        public void Validate(string[] fields)
        {
            if (fields.Length != 3)
            {
                throw new FieldLengthException();
            }

            if (fields[0].Length != 6)
            {
                throw new CurrencyException();
            }

            if (!int.TryParse(fields[1], out var t))
            {
                throw new TradeAmountException();
            }

            if (!decimal.TryParse(fields[2], NumberStyles.Number, new CultureInfo("en-US"), out var tradePrice))
            {
                throw new TradePriceException();
            }
        }
    }
}
