using System;
using System.Collections.Generic;
using System.Linq;

namespace UC.ALG.SearchingBenchmark
{
    public class SortedArray : IBenchableStructure
    {
        public List<int> Inner { get; }
        // Creates a new, empty instance.
        public SortedArray()
        {
            Inner = new List<int>();
        }

        // Searches for +value+ in +self+.
        // Returns index of +value+ in the array or -1 if +value+ is not found.
        public int Find(int value)
        {
            return BinarySearch(value);
        }

        // Searches for +value+ using binary search.
        // Returns index of +value+ in the array or -1 if +value+ is not found.
        public int BinarySearch(int value)
        {
            var searchResult = BinarySearchInternal(value);
            return searchResult.Item1 ? searchResult.Item2 : -1;
        }

        // Searches for +value+ using interpolation search.
        // Returns index of +value+ in the array or -1 if +value+ is not found.
        public int InterpolationSearch(int value)
        {
            var searchResult = InterpolationSearchInternal(value);
            return searchResult.Item1 ? searchResult.Item2 : -1;
        }

        // Inserts +value+ into +self+. If +value+ is already there, this method does nothing.
        // Returns index of +value+.
        public int Insert(int value)
        {
            var searchResult = BinarySearchInternal(value);
            if (!searchResult.Item1)
            {
                Inner.Insert(searchResult.Item2,value);
            }

            return searchResult.Item2;
        }

        // Deletes +value+ from +self+. If +self+ doesn't contain +value+, this method does nothing.
        // Returns +true+ if +value+ was deleted, returns false if +value+ was not found.
        public bool Delete(int value)
        {
            var index = BinarySearch(value);
            if (index == -1)
            {
                return false;
            }
            Inner.RemoveAt(index);
            return true;
        }

        // Converts self into a regular +Array+.
        public int[] to_a()
        {
            return Inner.ToArray();
        }



        // Searches for +value+ using binary search. Only for internal use, shouldn't be used outside this class.
        // Returns a two-item array. If +value+ is found, returns +[true, index]+ where +index+ is the index of +value+.
        // Otherwise, returns +[false, index]+ where +index+ is the index to which +value+ could be inserted
        // (keeping the array sorted).
        private ValueTuple<bool,int> BinarySearchInternal(int value)
        {
            if (Inner.Count== 0)
            {
                return (false, 0);
            }

            var left = 0;
            var right = Inner.Count- 1;
            if (value < Inner[left])
            {
                return (false, 0);
            }

            if (value > Inner[right])
            {
                return (false, right + 1);
            }
            while (left <= right)
            {
                var middle = (left + right) / 2;
                if (Inner[middle] == value)
                {
                    return (true, middle);
                }
                else if (value < Inner[middle])
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
        private ValueTuple<bool,int> InterpolationSearchInternal(int value)
        {
            if (Inner.Count== 0)
            {
                return (false, 0);
            }

            var left = 0;
            var right = Inner.Count- 1;
            if (value < Inner[left])
            {
                return (false, 0);
            }

            if (value > Inner[right])
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
                    candidate = (int)Math.Round(left + (right - left) * (value - Inner[left]) / (float)(Inner[right] - Inner[left]));
                }

                if (candidate < left)
                {
                    return (false, left);
                }

                if (candidate > right)
                {
                    return (false, right + 1);
                }

                if (Inner[candidate] == value)
                {
                    return (true, candidate);
                }

                else if (value < Inner[candidate])
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