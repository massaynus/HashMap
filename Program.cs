using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace HashMap
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> dic = new();
            HashTable<string> table = new();

            int curr = 1;
            foreach (var n in Enumerable.Range(0, 1000))
            {
                curr = table.nextPrimeNumber(curr);
                Console.WriteLine(curr);
            }

            foreach (var i in Enumerable.Range(0, (int)(127 * .7)))
            {
                var str = $"Yassine{i}";
                var res = table.NormalizeIndex(str);

                if (dic.ContainsKey(res)) dic[res]++;
                else dic[res] = 1;
            }

            foreach (var pair in dic.Where(d => d.Value > 1).OrderByDescending(d => d.Value))
            {
                Console.WriteLine($"{{ {pair.Key} : {pair.Value} }}");
            }

        }
    }
}
