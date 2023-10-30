using ChatKid.Application.Models.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Application.Models.ViewModels.AdvertisingViewModels
{
    public class AdvertisingDetailViewModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Company { get; set; }
        public string? CompanyEmail { get; set; }
        public decimal? Price { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ImageUrl { get; set; }

        public string? DestinationUrl { get; set; }
        public string? Type { get; set; }

        public short? Clicks { get; set; }
        public short? Status { get; set; }
        public AdminViewModel? CreateAdmin { get; set; }
    }
}
