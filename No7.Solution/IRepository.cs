using System.Collections.Generic;

namespace No7.Solution.Console
{
    public interface IRepository
    {
        void Create(string[] fields);
        int NumberOfTreads { get; }
    }
}
