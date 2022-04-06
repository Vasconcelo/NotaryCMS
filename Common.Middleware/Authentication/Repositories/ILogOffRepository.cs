namespace Common.Middleware.Authentication.Repositories
{
    using System.Threading.Tasks;

    public interface ILogOffRepository
    {
        Task<LogOffEntity> RetrieveAsync(string partitionKey, string rowKey);
    }
}
