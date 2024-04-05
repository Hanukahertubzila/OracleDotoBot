using OracleDotoBot.DAL.Entities;

namespace OracleDotoBot.Abstractions.Services
{
    public interface IUsersService
    {
        List<User> Users { get; set; }

        List<string> VipUsers { get; set; }

        Task<bool> CheckSubscription(long id);

        Task UpdateActiveUsersList();

        Task<string> GetSubscriptionPeriod(long id);

        Task<string> UpdateSubscription(int daysCount, long id);

        Task<int> GetTotalUserCount();
    }
}