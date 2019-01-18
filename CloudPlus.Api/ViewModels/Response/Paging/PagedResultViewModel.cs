using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using CloudPlus.Extensions.Queryable;

namespace CloudPlus.Api.ViewModels.Response.Paging
{
    public class PagedResultViewModel<T> : IHttpActionResult
    {
        private readonly IQueryable<T> _collection;
        private readonly string _orderBy;
        private readonly string _orderType;
        private readonly int _pageSize;
        private readonly HttpRequestMessage _request;
        private readonly string _routeName;
        private int _page;

        public PagedResultViewModel(HttpRequestMessage request, IQueryable<T> collection,
            int page,
            int pageSize,
            string orderBy,
            string orderType,
            string routeName)
        {
            _request = request;
            _collection = collection;
            _page = page;
            _pageSize = pageSize;
            _orderBy = orderBy;
            _orderType = orderType;
            _routeName = routeName;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            _page = _page == 0 ? 1 : _page;

            var skip = _pageSize * (_page - 1);
            var orderByColumn = typeof(T).GetProperty(_orderBy);

            if (orderByColumn == null)
                throw new Exception("Invalid order column!");

            var result = _collection.OrderByProperty(_orderBy, _orderType).Skip(skip).Take(_pageSize);

            var total = _collection.Count();

            var offset = total % _pageSize == 0 ? 0 : 1;
            var pageCount = total / _pageSize + offset;

            var nextPageUrl = _page == total
                ? null
                : new UrlHelper(_request).Link(_routeName, new
                {
                    page = _page + 1,
                    pageSize = _pageSize
                });
            var response = _request.CreateResponse(HttpStatusCode.OK, new PagedResultContent<T>
            {
                Results = result,
                PageNumber = _page,
                PageSize = result.Count(),
                TotalNumberOfPages = pageCount,
                TotalNumberOfRecords = total,
                NextPageUrl = nextPageUrl
            });

            return Task.FromResult(response);
        }
    }
}