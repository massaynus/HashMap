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
            var count = 10_000;
            HashTable<int, string> table = new((int)(count * 1.35f));
            var str = "YY";
            Stopwatch addW = new();
            Console.WriteLine($"Test pool size: {count}");

            addW.Start();
            for (int i = 0; i < count; i++)
                table.Add(new(i, str));

            addW.Stop();
            Console.WriteLine($"avg addition: {addW.ElapsedTicks / count / 100} nano second");
            addW.Reset();

            addW.Start();
            for (int i = 0; i < count; i++)
                _ = table[i];
            addW.Stop();
            Console.WriteLine($"avg retrieval: {addW.ElapsedTicks / count / 100} nano second");

            Console.ReadLine();
        }
    }
}
