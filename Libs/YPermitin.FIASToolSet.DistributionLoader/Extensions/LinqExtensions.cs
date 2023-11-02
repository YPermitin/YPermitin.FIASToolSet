using System.Linq.Expressions;

namespace YPermitin.FIASToolSet.DistributionLoader.Extensions;

public static class LinqExtensions
{
    public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult> (
        this IQueryable<TOuter> outer,
        IQueryable<TInner> inner,
        Expression<Func<TOuter, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<JoinResult<TOuter, TInner>, TResult>> resultSelector) {
        var result = outer
            .GroupJoin(inner, outerKeySelector, innerKeySelector, (outer1, inners) => new { outer1, inners = inners.DefaultIfEmpty() })
            .SelectMany(row => row.inners, (row, inner1) => new JoinResult<TOuter, TInner> { Outer = row.outer1, Inner = inner1 })
            .Select(resultSelector);

        return result;
    }
    
    public class JoinResult<TOuter, TInner> {
        public TOuter Outer { get; set; }
        public TInner Inner { get; set; }
    }
}