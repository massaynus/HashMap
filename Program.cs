using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace HashMap
{
    class Program
    {
        static void Main(string[] args)
        {
            bool test = true;
            int count = 10000;

            Stopwatch watch = new();
            watch.Start();
            HashTable<int, string> t = new(count * 2);
            for (int n = 0; n < count; n++)
            {
                t.Add(n, $"Y{n}");
            }
            watch.Stop();

            while (true)
            {
                HashTable<int, string> table = new();
                foreach (var n in Enumerable.Range(0, count))
                {
                    table.Add(n, $"Y{n}");
                }

                foreach (var n in Enumerable.Range(0, count))
                {
                    test &= table[n] == $"Y{n}";
                }

                count *= 2;
                if (!test || count > 10_000_000)
                    break;
            }
        }
    }
}
