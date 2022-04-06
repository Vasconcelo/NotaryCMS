namespace Common.Middleware.Authentication.Repositories
{
    public interface IApplicationRepository
    {
        string RetrieveClientSecretById(string id);

        string RetrieveClientSecretByUri(string uri);
    }
}
