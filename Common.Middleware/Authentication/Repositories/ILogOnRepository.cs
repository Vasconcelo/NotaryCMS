namespace Common.Middleware.Authentication.Repositories
{
    using System.Threading.Tasks;

    public interface ILogOnRepository
    {
        Task<LogOnEntity> RetrieveAsync(string partitionKey, string rowKey);
    }
}
