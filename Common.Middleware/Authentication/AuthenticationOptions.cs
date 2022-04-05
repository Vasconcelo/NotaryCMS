namespace Common.Middleware.Authentication
{
    public class AuthenticationOptions
    {
        public string IdentityAdo { get; set; }

        public string Redis { get; set; }

        public string LogStorage { get; set; }

        public string OpsStorage { get; set; }

        public string ApiKeyKeySecretId { get; set; }

        public string ApiKeyIvSecretId { get; set; }

        public string ADAuthClientId { get; set; }

        public string ADAuthClientSecret { get; set; }
    }
}
