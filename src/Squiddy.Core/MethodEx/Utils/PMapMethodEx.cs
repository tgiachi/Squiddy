using System.Collections.Concurrent;
using Dasync.Collections;

namespace Squiddy.Core.MethodEx.Utils;

/// <summary>
///  Method that emulate p-map js library
/// </summary>
public static class PMapMethodEx
{
    public static async Task<IEnumerable<TResult>> PMapAsync<T, TResult>(
        this IEnumerable<T> source, int maxDegreeOfParallelism, Func<T, Task<TResult>> action
    )
    {
        var result = new ConcurrentBag<TResult>();
        await source.ParallelForEachAsync(
            async item =>
            {
                result.Add(await action(item));
            },
            maxDegreeOfParallelism
        );

        return result;
    }
}
