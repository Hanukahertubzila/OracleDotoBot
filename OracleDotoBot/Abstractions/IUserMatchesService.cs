namespace OracleDotoBot.Abstractions
{
    public interface IUserMatchesService
    {
        Task<string> AlterMatch(long chatId, int heroId);

        void NewMatch(long chatId);
    }
}