using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace UC.ALG.SearchingBenchmark
{
    public class Benchmark
    {
        private int ArraySize { get; }
        private int Repeat { get; }
        private List<int[]> RefListInserting { get; set; }
        private List<int[]> RefListSearching { get; set; }
        private List<int[]> RefListDeleting { get; set; }

        public Benchmark(int arraySize, int repeat)
        {
            ArraySize = arraySize;
            Repeat = repeat;
        }
        public Dictionary<BenchmarkStructureType, Dictionary<BenchmarkOperationType, TimeSpan>> Start()
        {
            SetUp();
            WarmUp();

            RefListInserting = new List<int[]>();
            for (int i = 0; i < Repeat; i++)
            {
                RefListInserting.Add(FillArray(ArraySize));
            }
            RefListSearching = new List<int[]>();
            for (int i = 0; i < Repeat; i++)
            {
                RefListSearching.Add(FillArray(ArraySize));
            }
            RefListDeleting = new List<int[]>();
            for (int i = 0; i < Repeat; i++)
            {
                RefListDeleting.Add(FillArray(ArraySize));
            }

            var results = new Dictionary<BenchmarkStructureType, Dictionary<BenchmarkOperationType, TimeSpan>>();
            foreach (BenchmarkStructureType structure in Enum.GetValues(typeof(BenchmarkStructureType)))
            {
                results.Add(structure, new Dictionary<BenchmarkOperationType, TimeSpan>());
                foreach (BenchmarkOperationType operation in Enum.GetValues(typeof(BenchmarkOperationType)))
                {
                     results[structure].Add(operation, BenchAlgorithm(structure, operation));
                }
            }

            return results;
        }

        private void SetUp()
        {
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2); //použití druhého jádra/procesoru pro měření
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; //zabrání procesům s prioritou "Normal" přerušit tento proces
            Thread.CurrentThread.Priority = ThreadPriority.Highest; //zabrání vláknům s nižší prioritou přerušit toto vlákno
        }

        private TimeSpan BenchAlgorithm(BenchmarkStructureType type, BenchmarkOperationType operation)
        {
            Action<int[]> sortFunc;
            IBenchableStructure structure;
            switch (type)
            {
                case BenchmarkStructureType.UnsortedArray:
                    structure = ne
                    break;
                case BenchmarkStructureType.SortedArray:
                    break;
                case BenchmarkStructureType.BinarySearchTree:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var list = CopyArrayList(RefList);
            var sw = new Stopwatch();
            sw.Start();
            foreach (var array in list)
            {
                sortFunc(array);
            }
            sw.Stop();
            return sw.Elapsed;
        }

        private TimeSpan B<T>()
        {
            T
        }
        private List<int[]> CopyArrayList(List<int[]> refList)
        {
            var list = new List<int[]>(refList.Count);
            foreach (var array in refList)
            {
                list.Add(array.ToArray());
            }

            return list;
        }

        private void WarmUp()
        {
            var sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 1200)
            {

                Array.Sort(FillArray(5000));
            }
            sw.Stop();
        }

        private int[] FillArray(int size, int randomMax = int.MaxValue)
        {
            int[] array = new int[size];
            Random random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, randomMax);
            }
            return array;
        }
    }
}
