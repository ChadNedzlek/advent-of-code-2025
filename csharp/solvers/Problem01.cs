using System;
using System.Linq;
using System.Threading.Tasks;
using ChadNedzlek.AdventOfCode.Library;

namespace ChadNedzlek.AdventOfCode.Y2025.CSharp;

public class Problem01 : AsyncProblemBase
{
    protected override async Task ExecuteCoreAsync(string[] data)
    {
        var countOfZero = data.As<char, long>(@"^(L|R)(\d+)$")
            .Select(((char dir, long amount) p) => p.dir == 'L' ? -p.amount : p.amount)
            .RunningAggregate(50L, (value, current) => ((current % 100) + 100 + value) % 100)
            .Count(x => x == 0);
        
        Console.WriteLine($"Hit zero {countOfZero} times");
        
        var passedZero = data.As<char, long>(@"^(L|R)(\d+)$")
            .Select(((char dir, long amount) p) => p.dir == 'L' ? -p.amount : p.amount)
            .AggregateSelect(50L, (value, current) =>
                {
                    var next = current + value;
                    
                    long rots = 0;
                    while (next < 0)
                    {
                        rots++;
                        next += 100;
                    }

                    while (next > 100)
                    {
                        rots++;
                        next -= 100;
                    }

                    if (next is 0 or 100)
                    {
                        rots++;
                        next = 0;
                    }

                    if (current == 0 && value < 0) rots--;

                    if (rots != 0)
                    {
                        Helpers.VerboseLine($"When rotating from {current} by {value} to {next} passed zero {rots} times.");
                    }

                    return (rots, next);
                }
            )
            .Sum();
        
        Console.WriteLine($"Passed zero {passedZero} times");
    }
}