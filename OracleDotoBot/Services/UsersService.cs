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
            _usersRepository = usersRepository;
        }

        public List<User> Users { get; set; }

        private readonly UsersRepository _usersRepository;

        public async Task UpdateActiveUsersList()
        {
            var users = await _usersRepository.GetAllWithActiveSubscription();

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

            user = await _usersRepository.Add(id, DateTime.Now.AddDays(0), DAL.Enums.Roles.User);
            Users.Add(user);

            return true;
        }

    }
}
