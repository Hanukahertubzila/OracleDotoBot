using Microsoft.EntityFrameworkCore;
using OracleDotoBot.DAL.Entities;
using OracleDotoBot.DAL.Enums;

namespace OracleDotoBot.DAL.Repositories
{
    public class UsersRepository 
    {
        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        private readonly UsersDbContext _context;

        public async Task<List<User>> GetAllWithActiveSubscription()
        {
            var users = await _context.Users
                .Where(u => u.SubscriptionEndTime > DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

        public async Task<User?> GetById(long id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> Add(long id, DateTime subscritptionEndTime, Roles role)
        {
            var user = new User()
            {
                Id = id,
                SubscriptionEndTime = subscritptionEndTime,
                Role = role
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task Update(long id, DateTime subscriptionEndTime)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.SubscriptionEndTime = subscriptionEndTime;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
