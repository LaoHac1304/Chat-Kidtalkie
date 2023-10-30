using ChatKid.DataLayer.Entities;

namespace ChatKid.Application.Models.ViewModels.WalletViewModels
{
    public class WalletViewModel
    {
        public Guid Id { get; set; }

        public short? TotalEnergy { get; set; }

        public Guid? OwnerId { get; set; }

        public short? Status { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
