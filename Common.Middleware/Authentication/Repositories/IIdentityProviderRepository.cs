namespace Common.Middleware.Authentication.Repositories
{
    public interface IIdentityProviderRepository
    {
        IdentityProvider RetrieveByIssuerUri(string issuerUri);
    }
}
