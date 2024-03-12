namespace UnitOfWork.Core.Models
{
    public abstract class BasePagingRequest
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = int.MaxValue;
    }
}
