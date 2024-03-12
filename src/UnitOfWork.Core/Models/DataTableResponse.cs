﻿namespace UnitOfWork.Core.Models
{
    public class DataTableResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}
