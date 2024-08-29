using System.Linq;

namespace ProjetoTemplate.Domain.Helpers
{
    public static class GenericFunctions
    {
        public static IQueryable<TSource> RWSPaginate<TSource>(this IQueryable<TSource> data, int pageIndex, int pageSize)
        {
            return data.Skip(pageSize * pageIndex).Take(pageSize);
        }
    }
}
