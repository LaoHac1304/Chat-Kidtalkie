using ChatKid.DataLayer.Entities;

namespace ChatKid.DataLayer.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> IsFullFamilyUser(Guid familyId);
        Task<User> LoginWithPassword(Guid id, string password);
    }
}
