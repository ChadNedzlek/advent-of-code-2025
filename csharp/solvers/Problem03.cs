using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChadNedzlek.AdventOfCode.Library;

namespace ChadNedzlek.AdventOfCode.Y2025.CSharp;

public class Problem03 : AsyncProblemBase
{
    protected override async Task ExecuteCoreAsync(string[] data)
    {
        Console.WriteLine($"Basic joltage {Joltify(data, 2)}");
        Console.WriteLine($"Fancy joltage {Joltify(data, 12)}");
    }

    private static long Joltify(string[] data, int count)
    {
        long sum = 0;
        foreach (var row in data)
        {
            byte[] joltages = row.Select(b => (byte)(b - '0')).ToArray();
            long best = joltages.Take(count).Aggregate(0L, (current, digit) => current * 10 + digit);

            Helpers.VerboseLine($"Starting joltage: {best}");
            foreach (byte digit in joltages.Skip(count))
            {
                var starting = best;
                Helpers.Verbose($" ");
                for (int i = 0; i < count; i++)
                {
                    long toRep = 10L.Pow(i);
                    long leftBits = starting / toRep / 10 * 10 * toRep;
                    long rightBits = starting % toRep * 10;
                    long mergeIn = digit;
                    long candidate = leftBits + rightBits + mergeIn;
                    Helpers.Verbose($"{candidate}|");
                    best = long.Max(best, candidate);
                }
                Helpers.Verbose($"=>{best}");
            }
            Helpers.VerboseLine();
            Helpers.VerboseLine($"Best cells values are {best}");
            sum += best;
        }

        return sum;
    }
}

public class ReverseCompare<T>(Comparer<T> comp) : IComparer<T>
{
    public int Compare(T x, T y) => -comp.Compare(x, y);
}

public static class ReverseCompare
{
    public static IComparer<T> Of<T>(Comparer<T> comp) => new ReverseCompare<T>(comp);
    public static IComparer<T> Reversed<T>(this Comparer<T> comp) => new ReverseCompare<T>(comp);
}