using System;

namespace UC.ALG.SearchingBenchmark
{
    public interface IBenchableStructure
    {
        int Insert(int value);
        bool Delete(int value);
        int Find(int value);
    }
}