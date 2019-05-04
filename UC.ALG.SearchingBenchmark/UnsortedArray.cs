using System.Collections.Generic;

namespace UC.ALG.SearchingBenchmark
{
    public class UnsortedArray
    {
        public List<int> Inner { get; }
        // Creates a new, empty instance.
        public UnsortedArray()
        {
            Inner = new List<int>();
        }

        // Searches for +value+ in +self+.
        // Returns index of +value+ in the array or -1 if +value+ is not found.
        public int Find(int value)
        {
            var result = -1;
            for (int i = 0; i < Inner.Count; i++)
            {
                var item = Inner[i];
                if (value == item)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        // Inserts +value+ into +self+. If +value+ is already there, this method does nothing.
        // Returns index of +value+.
        public int Insert(int value)
        {
            var index = Find(value);
            if (index == -1)
            {
                Inner.Add(value);
                index = Inner.Count - 1;
            }

            return index;
        }

        // Deletes +value+ from +self+. If +self+ doesn't contain +value+, this method does nothing.
        // Returns +true+ if +value+ was deleted, returns false if +value+ was not found.
        public bool Delete(int value)
        {
            var index = Find(value);
            if (index == -1)
            {
                return false;
            }

            Inner[index] = Inner[Inner.Count - 1];
            Inner.RemoveAt(Inner.Count - 1);
            return true;
        }

        // Converts self into a regular +Array+.
        public int[] ToArray()
        {
            return Inner.ToArray();
        }
    }
}