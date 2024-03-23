using OracleDotoBot.DAL.Enums;

namespace OracleDotoBot.DAL.Entities
{
    public class User
    {
        public long Id { get; set; }

        public DateTime SubscriptionEndTime { get; set; }

        public Roles Role { get; set; }
    }
}
