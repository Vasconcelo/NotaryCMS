namespace Common.Middleware.Authentication.Repositories
{
    public interface IUserAccountRepository
    {
        UserAccount RetrieveByUserAppId(string id);
    }
}
