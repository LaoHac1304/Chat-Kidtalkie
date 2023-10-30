using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.Application.Models.FilterModels
{
    public class AdvertisingFilter
    {
        public bool isRandom { get; set; } = false;
        public string? Type { get; set; }
    }
}
