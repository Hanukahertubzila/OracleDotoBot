using OracleDotoBot.Abstractions.Services;
using OracleDotoBot.DAL.Entities;
using OracleDotoBot.DAL.Repositories;

namespace OracleDotoBot.Services
{
    public class UsersService : IUsersService
    {
        public UsersService(UsersRepository usersRepository)
        {
            Users = new List<User>();
            VipUsers = new List<string>();
            _usersRepository = usersRepository;
        }

        public List<User> Users { get; set; }

        public List<string> VipUsers { get; set; }

        private readonly UsersRepository _usersRepository;

        public async Task UpdateActiveUsersList()
        {
            var users = await _usersRepository.GetAllWithActiveSubscription();
            var admin = users.FirstOrDefault(u => u.Id == 1059375760);
            if (admin != null)
                admin.Role = DAL.Enums.Roles.Admin;
            else
            {
                var a = await _usersRepository.Add(1059375760, DateTime.UtcNow.AddDays(30), DAL.Enums.Roles.Admin);
                users.Add(a);
            }
            Users = users;
        }

        public async Task<bool> CheckSubscription(long id)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
                return user.SubscriptionEndTime > DateTime.Now;

            user = await _usersRepository.GetById(id);
            if (user != null)
                return user.SubscriptionEndTime > DateTime.Now;

            user = await _usersRepository.Add(id, DateTime.Now.AddDays(2), DAL.Enums.Roles.User);
            Users.Add(user);

            return true;
        }

        public async Task<string> GetSubscriptionPeriod(long id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                user = await _usersRepository.GetById(id);
            }

            if (user.SubscriptionEndTime < DateTime.Now)
                return "Ваша подписка закончилась...";
            return $"Ваша подписка продлится до {user.SubscriptionEndTime.AddHours(3).ToString("dd.MM.yy H:mm")} по Мск";
        }

        public async Task<int> GetTotalUserCount()
        {
            var count = await _usersRepository.GetTotalUserCount();

            return count;
        }

        public async Task<string> UpdateSubscription(int daysCount, long id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                user = await _usersRepository.GetById(id);
            }

            if (user.SubscriptionEndTime > DateTime.UtcNow)
            {
                user.SubscriptionEndTime = user.SubscriptionEndTime.AddDays(daysCount);
                await _usersRepository.UpdateSubscription(id, user.SubscriptionEndTime);
                if (!Users.Contains(user))
                {
                    Users.Add(user);
                }
            }
            else
            {
                user.SubscriptionEndTime = DateTime.UtcNow.AddDays(daysCount);
                await _usersRepository.UpdateSubscription(id, DateTime.UtcNow.AddDays(daysCount));
            }
            return $"Ваша подписка продлится до {user.SubscriptionEndTime.AddHours(3).ToString("dd.MM.yy H:mm")} по Мск";
        }
    }
}
