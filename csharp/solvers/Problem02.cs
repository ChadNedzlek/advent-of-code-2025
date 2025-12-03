using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChadNedzlek.AdventOfCode.Library;

namespace ChadNedzlek.AdventOfCode.Y2025.CSharp;

public class Problem02 : AsyncProblemBase
{
    protected override async Task ExecuteCoreAsync(string[] data)
    {
        var magic = data[0]
            .Split(',')
            .As<long, long>(@"^(\d+)-(\d+)$")
            .SelectMany(SplitRanges)
            .SelectMany(range => GenMagicNumber(range.start)
                .SelectMany(n =>
                {
                    if (!ValidFor(range.start, n))
                    {
                        // Helpers.VerboseLine($"For range {range.start} to {range.end} is invalid range for {n}");
                        return [];
                    }
                    
                    long roundStart = (range.start / n) * n;
                    if (roundStart != range.start)
                        roundStart += n;
                    long roundEnd = (range.end / n) * n;
                    return Helpers.RangeInclusive(roundStart, roundEnd, n)
                        .Select(k =>
                            {
                                // Helpers.VerboseLine($"For range {range.start} to {range.end} and magic {n} we hit {k}");
                                return k;
                            }
                        );
                })
            )
            .Sum();
        
        Console.WriteLine($"Magic values passed = {magic}");
        var highest = data[0]
            .Split(',')
            .As<long, long>(@"^(\d+)-(\d+)$")
            .Max(r => r.Item2);
        Helpers.VerboseLine($"Magic numbers = {string.Join(',', GenMagicNumbers(highest))}");
        
        
        var magic2 = data[0]
            .Split(',')
            .As<long, long>(@"^(\d+)-(\d+)$")
            .SelectMany(SplitRanges)
            .SelectMany(range => GenMagicNumbers(range.end)
                .SelectMany(n =>
                {
                    if (!ValidFor(range.start, n))
                    {
                        // Helpers.VerboseLine($"For range {range.start} to {range.end} is invalid range for {n}");
                        return [];
                    }

                    long roundStart = (range.start / n) * n;
                    if (roundStart != range.start)
                        roundStart += n;
                    long roundEnd = (range.end / n) * n;
                    return Helpers.RangeInclusive(roundStart, roundEnd, n)
                        .Select(k =>
                            {
                                Helpers.VerboseLine($"For range {range.start} to {range.end} and magic {n} we hit {k}");
                                return k;
                            }
                        );
                })
            )
            .Distinct()
            .Sum();
        
        Console.WriteLine($"Magicer values passed = {magic2}");
        return;

        IEnumerable<(long start, long end)> SplitRanges((long start, long end) range)
        {
            long start = range.start;
            long end = range.end;
            while ((int)Math.Floor(Math.Log10(start)) != (int)Math.Floor(Math.Log10(end)))
            {
                long split = (long)Math.Pow(10, (int)Math.Floor(Math.Log10(start) + 1));
                yield return (start, split-1);
                start = split;
            }

            yield return (start, end);
        }

        static bool ValidFor(long number, long pattern)
        {
            pattern /= 10;
            number /= 10;
            while (pattern % 10 == 0)
            {
                pattern /= 10;
                number /= 100;
            }

            return number != 0 &&
                (long)Math.Log10(pattern) == (long)Math.Log10(number);
        }
    }

    private IEnumerable<long> GenMagicNumber(long min)
    {
        min -= 1;
        long mult = 10;
        while (true)
        {
            var next = mult + 1;
            if (next >= Math.Sqrt(min))
            {
                return [mult + 1];
            }
            mult *= 10;
        }
    }
    
    private IEnumerable<long> GenMagicNumbers(long min)
    {
        return Recurse(1);

        IEnumerable<long> Recurse(long i)
        {
            for (long m = 10; m * i < min; m *= 10)
            {
                long magic = m * i + 1;
                yield return magic;
                for (long m2 = m; magic * m < min; m2 *= m)
                {
                    magic *= m;
                    magic++;
                    yield return magic;
                }
            }
        }
    }
}