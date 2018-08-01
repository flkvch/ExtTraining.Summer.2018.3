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
    internal class Parser : IParser
    {
        public TradeRecord Parse(string[] fields)
        {
            IValidator validator = new Validator();
            try
            {
                validator.Validate(fields);
            }
            catch (FieldLengthException)
            {
                throw;
            }
            catch (CurrencyException)
            {
                throw;
            }
            catch (TradeAmountException)
            {
                throw;
            }
            catch (TradePriceException)
            {
                throw;
            }

            int tradeAmount = int.Parse(fields[1]);
           
            decimal tradePrice = decimal.Parse(fields[2], new CultureInfo("en-US"));

            var trade = new TradeRecord
            {
                SourceCurrency = fields[0].Substring(0, 3),
                DestinationCurrency = fields[0].Substring(3, 3),
                Lots = tradeAmount / TradeService.LotSize,
                Price = tradePrice
            };

            return trade;
        }
    }
}
