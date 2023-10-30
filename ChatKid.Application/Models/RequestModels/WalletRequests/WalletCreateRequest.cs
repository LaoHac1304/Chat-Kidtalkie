namespace ChatKid.Application.Models.RequestModels.WalletRequests
{
    public class WalletCreateRequest
    {
        public short? TotalEnergy { get; set; }

        public Guid? OwnerId { get; set; }
    }
}
