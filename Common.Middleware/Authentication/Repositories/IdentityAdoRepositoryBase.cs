namespace Common.Middleware.Authentication.Repositories
{
    using Microsoft.Extensions.Options;

    public abstract class IdentityAdoRepositoryBase
    {
        protected const int MaxRetries = 4;
        protected const int MinBackoffDelay = 2000;
        protected const int MaxBackoffDelay = 8000;
        protected const int DeltaBackoffDelay = 2000;

        protected IdentityAdoRepositoryBase(IOptions<AuthenticationOptions> options)
        {
            ConnectionString = options.Value.IdentityAdo;
        }

        protected string ConnectionString { get; private set; }
    }
}
