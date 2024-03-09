using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Extensions.DependencyInjection;

public static class QueryableExtensions
{
    public static async Task<HashSet<TSource>> ToHashSetAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        var set = new HashSet<TSource>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken)
                           .ConfigureAwait(false))
        {
            set.Add(element);
        }

        return set;
    }
}