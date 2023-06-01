using Election2023.Application.Specifications.Base;
using Election2023.Domain.Common;

namespace Election2023.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> specification) where T : class, IEntity
    {
        var includeResult = specification.Includes
            .Aggregate(query, (current, include) => current.Include(include));

        var secodaryResult = specification.IncludeStrings
            .Aggregate(includeResult, (current, include) => current.Include(include));

        return secodaryResult.Where(specification.Criteria);
    }
}