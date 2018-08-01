using System.Collections.Generic;
using System.IO;

namespace No7.Solution.Interfaces
{
    internal interface IDataProvider
    {
        IEnumerable<string> ProvideData(Stream stream);
    }
}
