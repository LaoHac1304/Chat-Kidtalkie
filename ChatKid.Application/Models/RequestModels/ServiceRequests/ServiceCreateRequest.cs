namespace ChatKid.Application.Models.RequestModels.ServiceRequests
{
    public class ServiceCreateRequest
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public short? Energy { get; set; }
    }
}
