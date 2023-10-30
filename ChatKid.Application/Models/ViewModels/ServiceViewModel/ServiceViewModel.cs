namespace ChatKid.Application.Models.ViewModels.ServiceViewModel
{
    public class ServiceViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public short? Energy { get; set; }

        public DateTime? CreatedTime { get; set; }

        public Guid? UpdatedBy { get; set; }

    }
}
