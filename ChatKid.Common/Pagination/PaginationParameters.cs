using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ChatKid.Common.Pagination
{
    public class PaginationParameters
    {
        public PaginationParameters() { }
        public PaginationParameters(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        [FromQuery(Name = "page-number")]
        public int PageNumber { get; set; } = 0;

        [FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = 10;
    }
}
