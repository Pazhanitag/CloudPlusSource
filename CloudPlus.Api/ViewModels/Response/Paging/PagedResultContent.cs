using System.Linq;

namespace CloudPlus.Api.ViewModels.Response.Paging
{
    public class PagedResultContent<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfRecords { get; set; }
        public string NextPageUrl { get; set; }
        public IQueryable<T> Results { get; set; }
    }
}