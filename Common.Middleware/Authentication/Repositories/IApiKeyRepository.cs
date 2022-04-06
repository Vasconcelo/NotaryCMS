namespace Common.Middleware.Authentication.Repositories
{
    using System;

    public interface IApiKeyRepository
    {
        ApiKeyApp RetrieveById(Guid id);

        string RetrieveKeyById(Guid id);
    }
}
