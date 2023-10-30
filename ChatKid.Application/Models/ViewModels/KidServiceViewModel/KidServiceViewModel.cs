namespace ChatKid.Application.Models.ViewModels.KidServiceViewModel
{
    public class KidServiceViewModel
    {
        public Guid Id { get; set; }

        public Guid? ChildrenId { get; set; }

        public Guid? ServiceId { get; set; }

        public short? Status { get; set; }
    }
}
