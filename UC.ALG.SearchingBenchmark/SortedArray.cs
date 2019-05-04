using System;
using System.Collections.Generic;
using System.Linq;

namespace UC.ALG.SearchingBenchmark
{
    public class SortedArray
    {
        public List<int> inner { get; }
        // Creates a new, empty instance.
        public SortedArray()
        {
            inner = new List<int>();
        }

        // Searches for +value+ in +self+.
        // Returns index of +value+ in the array or -1 if +value+ is not found.
        public int find(int value)
        {
            return binary_search(value);
        }

        // Searches for +value+ using binary search.
        // Returns index of +value+ in the array or -1 if +value+ is not found.
        public int binary_search(int value)
        {
            var search_result = binary_search_internal(value);
            return search_result.Item1 ? search_result.Item2 : -1;
        }

        // Searches for +value+ using interpolation search.
        // Returns index of +value+ in the array or -1 if +value+ is not found.
        public int interpolation_search(int value)
        {
            var search_result = interpolation_search_internal(value);
            return search_result.Item1 ? search_result.Item2 : -1;
        }

        // Inserts +value+ into +self+. If +value+ is already there, this method does nothing.
        // Returns index of +value+.
        public int insert(int value)
        {
            var search_result = binary_search_internal(value);
            if (!search_result.Item1)
            {
                inner.Insert(search_result.Item2,value);
            }

            return search_result.Item2;
        }

        // Deletes +value+ from +self+. If +self+ doesn't contain +value+, this method does nothing.
        // Returns +true+ if +value+ was deleted, returns false if +value+ was not found.
        public bool delete(int value)
        {
            var index = binary_search(value);
            if (index == -1)
            {
                return false;
            }
            inner.RemoveAt(index);
            return true;
        }

        // Converts self into a regular +Array+.
        public int[] to_a()
        {
            return inner.ToArray();
        }



        // Searches for +value+ using binary search. Only for internal use, shouldn't be used outside this class.
        // Returns a two-item array. If +value+ is found, returns +[true, index]+ where +index+ is the index of +value+.
        // Otherwise, returns +[false, index]+ where +index+ is the index to which +value+ could be inserted
        // (keeping the array sorted).
        private ValueTuple<bool,int> binary_search_internal(int value)
        {
            if (inner.Count== 0)
            {
                return (false, 0);
            }

            var left = 0;
            var right = inner.Count- 1;
            if (value < inner[left])
            {
                return (false, 0);
            }

            if (value > inner[right])
            {
                return (false, right + 1);
            }
            while (left <= right)
            {
                var middle = (left + right) / 2;
                if (inner[middle] == value)
                {
                    return (true, middle);
                }
                else if (value < inner[middle])
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return (false, left);
        }

        // Searches for +value+ using interpolation search. Only for internal use, shouldn't be used outside this class.
        // Returns a two-item array. If +value+ is found, returns +[true, index]+ where +index+ is the index of +value+.
        // Otherwise, returns +[false, index]+ where +index+ is the index to which +value+ could be inserted
        // (keeping the array sorted).
        public ValueTuple<bool,int> interpolation_search_internal(int value)
        {
            if (inner.Count== 0)
            {
                return (false, 0);
            }

            var left = 0;
            var right = inner.Count- 1;
            if (value < inner[left])
            {
                return (false, 0);
            }

            if (value > inner[right])
            {
                return (false, right + 1);
            }
            while (left <= right)
            {
                int candidate;
                if (left == right)
                {
                    candidate = left;
                }
                else
                {
                    candidate = (int)Math.Round(left + (right - left) * (value - inner[left]) / (float)(inner[right] - inner[left]));
                }

                if (candidate < left)
                {
                    return (false, left);
                }

                if (candidate > right)
                {
                    return (false, right + 1);
                }

                if (inner[candidate] == value)
                {
                    return (true, candidate);
                }

                else if (value < inner[candidate])
                {
                    right = candidate - 1;
                }
                else
                {
                    left = candidate + 1;
                }
            }
            return (false, left);
        }
    }
}