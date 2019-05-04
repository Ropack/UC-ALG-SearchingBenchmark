using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace UC.ALG.SearchingBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var assignment = new List<ValueTuple<int, int>>()
            {
                (10, 50000),
                (50, 10000),
                (100, 5000),
                (500, 1000),
                (1000, 100),
                (50000, 10)
            };
            var results = new Dictionary<int, Dictionary<BenchmarkStructureType, TimeSpan>>();

            foreach (var values in assignment)
            {
                var benchmark = new Benchmark(values.Item1, values.Item2);
                results.Add(values.Item1, benchmark.Start());
            }

            WriteResults(assignment, results);
        }

        private static void WriteResults(List<(int, int)> assignment, Dictionary<int, Dictionary<BenchmarkStructureType, TimeSpan>> results)
        {
            using (var swTimes = new StreamWriter("times.txt"))
            using (var swRatios = new StreamWriter("ratios.txt"))
            {
                var tableFormat = "{0,-10}{1,-10}{2,-10}{3,-10}{4,-10}{5,0}";
                var figureFormat = "0.00e0";
                var header = string.Format(tableFormat, "# m", "UA_insert", "SA_insert", "BST_insert", "UA_search", "SA_binarys", "SA_interps", "BST_search", "UA_delete", "SA_delete", "BST_delete");
                swTimes.WriteLine(header);
                swRatios.WriteLine(header);
                foreach (var values in assignment)
                {
                    var averages = new[]
                    {
                        values.Item1.ToString(),
                        GetAverageTime(results[values.Item1][BenchmarkStructureType.UnsortedArray].Milliseconds, values.Item2)
                            .ToString(figureFormat, CultureInfo.InvariantCulture),
                        GetAverageTime(results[values.Item1][BenchmarkStructureType.SortedArray].Milliseconds, values.Item2)
                            .ToString(figureFormat, CultureInfo.InvariantCulture),
                        GetAverageTime(results[values.Item1][BenchmarkStructureType.BinarySearchTree].Milliseconds, values.Item2)
                            .ToString(figureFormat, CultureInfo.InvariantCulture)
                    };
                    swTimes.WriteLine(tableFormat, averages);
                    var ratios = new[]
                    {
                        values.Item1.ToString(),
                        GetRatio(results[values.Item1][BenchmarkStructureType.ShellK1].Milliseconds, values.Item2, values.Item1)
                            .ToString(figureFormat, CultureInfo.InvariantCulture),
                        GetRatio(results[values.Item1][BenchmarkStructureType.ShellK2].Milliseconds, values.Item2, values.Item1)
                            .ToString(figureFormat, CultureInfo.InvariantCulture),
                        GetRatio(results[values.Item1][BenchmarkStructureType.ShellK3].Milliseconds, values.Item2, values.Item1)
                            .ToString(figureFormat, CultureInfo.InvariantCulture),
                        GetRatio(results[values.Item1][BenchmarkStructureType.Insertion].Milliseconds, values.Item2,
                                values.Item1)
                            .ToString(figureFormat, CultureInfo.InvariantCulture),
                        GetRatio(results[values.Item1][BenchmarkStructureType.Quick].Milliseconds, values.Item2, values.Item1)
                            .ToString(figureFormat, CultureInfo.InvariantCulture),
                    };

                    swRatios.WriteLine(tableFormat, ratios);
                }
            }
        }

        private static double GetAverageTime(int totalTime, int repeat)
        {
            return totalTime / (double)repeat;
        }
        private static double GetRatio(int totalTime, int repeat, int arraySize)
        {
            return GetAverageTime(totalTime, repeat) / arraySize / Math.Log10(arraySize);
        }
    }
}
