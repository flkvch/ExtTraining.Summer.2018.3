using System.Collections.Generic;

namespace No7.Solution.Interfaces
{
    internal interface IStore
    {
        void Save(IEnumerable<TradeRecord> trades);
    }
}
