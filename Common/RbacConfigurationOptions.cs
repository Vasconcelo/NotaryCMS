namespace Common
{
    public class RbacConfigurationOptions
    {
        public bool RbacAuthorizationEnabled { get; set; }
        public bool ClaimsAuthorizationEnabled { get; set; }
        public bool RbacOffice365UserAuthorizationEnabled { get; set; }
        public bool RbacDatabaseUserAuthorizationEnabled { get; set; }
        public bool RbacApiKeyUserAuthorizationEnabled { get; set; }
        public int RbacCacheExpirationSeconds { get; set; }
        //public bool RbacEnabled { get; set; }
        public string IdentityServiceUrl { get; set; }
        public bool RedisEnabled { get; set; }
        public string RedisHost { get; set; }
        public string RedisPassword { get; set; }
        public string RedisConnection { get; set; }
    }
}
