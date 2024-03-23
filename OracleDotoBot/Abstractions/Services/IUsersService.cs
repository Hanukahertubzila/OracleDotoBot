using OracleDotoBot.DAL.Entities;

namespace OracleDotoBot.Abstractions.Services
{
    public interface IUsersService
    {
        List<User> Users { get; set; }

        Task<bool> CheckSubscription(long id);

        Task UpdateActiveUsersList();
    }
}