namespace ChatKid.Application.Models.RequestModels.Admin
{
    public class AdminCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gmail { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
