using UnitOfWork.Core.Models;

namespace UnitOfWork.Core.Extension
{
    public static class DataExtension
    {
        public static DataTableResponse<T> GetDataTableResponse<T>(this IEnumerable<T> data, BasePagedInfoResponse basePagedList)
        {
            return new DataTableResponse<T>
            {
                Data = data,
                PageIndex = basePagedList.PageIndex,
                PageSize = basePagedList.PageSize,
                TotalCount = basePagedList.TotalCount,
                TotalPages = basePagedList.TotalPages,
                HasPreviousPage = basePagedList.HasPreviousPage,
                HasNextPage = basePagedList.HasNextPage
            };
        }
    }
}
