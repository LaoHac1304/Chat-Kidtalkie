namespace ChatKid.Application.Models.RequestModels.KidServiceRequests
{
    public class KidServiceUpdateRequest
    {

        public Guid? ChildrenId { get; set; }

        public Guid? ServiceId { get; set; }

        public short? Status { get; set; }
    }
}
