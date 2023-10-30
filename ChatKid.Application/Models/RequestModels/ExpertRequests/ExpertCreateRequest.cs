using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.RequestModels.ExpertRequests
{
    public class ExpertCreateRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gmail { get; set; }

        public string Phone { get; set; }

        public int Age { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

    }
}
