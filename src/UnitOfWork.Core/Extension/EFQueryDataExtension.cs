using System.Linq.Expressions;
using UnitOfWork.Core.Models;

namespace UnitOfWork.Core.Extension
{
    public static class EFQueryDataExtension
    {
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, bool getOnlyTotalCount = false)
        {
            if (source == null)
                return new PagedList<T>(new List<T>(), pageIndex, pageSize);

            //min allowed page size is 1
            pageSize = Math.Max(pageSize, 1);

            var count = source.Count();

            var data = new List<T>();

            if (!getOnlyTotalCount)
                data.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());

            return new PagedList<T>(data, pageIndex, pageSize, count);
        }


    }
}
