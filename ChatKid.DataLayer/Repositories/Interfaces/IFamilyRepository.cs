using ChatKid.Common.CommandResult;
using ChatKid.DataLayer.Entities;

namespace ChatKid.DataLayer.Repository.Interfaces
{
    public interface IFamilyRepository : IGenericRepository<Family>
    {
        Task<Family> GetByEmailAsync(string email);
    }
}
