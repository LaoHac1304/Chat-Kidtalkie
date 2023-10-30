using Microsoft.AspNetCore.Mvc;
using ChatKid.Common.Extensions;
namespace ChatKid.Application.Models.SearchFilter
{
    public class SearchFilter
    {
        [FromQuery(Name = "search")]
        public string? SearchString { get; set; } = "";

        [FromQuery(Name = "status")]
        public short? Status { get; set; } = 2;

        [FromQuery(Name = "email")]
        public string? Email { get; set; }

        public static string BuildSearchTerm(string searchText)
        {
            if (searchText.IsNullOrEmpty()) return "";
            return string.Join(" | ", searchText.Split(' '));
        }
    }
}
