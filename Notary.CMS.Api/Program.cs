using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notary.Api.DataAccess.Repositories;
using Notary.CMS.Api.DataAccess.Repositories;
using Notary.CMS.DataAccess.Interfaces;
using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Repositories;
using Newtonsoft.Json;
using Notary.CMS.Api.Model;
using Microsoft.AspNetCore.Builder;
using Common.Logging;
using Common.Middleware.Authentication;
using Common;
using Common.Filters;
using Common.Security;
//using Common.Middleware.Correlation;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;

var defaultNNASchema = "DefaultNNASchema";

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Data Database configuration
var applicationConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NotaryCMSDBContext>(options => options.UseSqlServer(applicationConnectionString));

builder.Services.Configure<Common.Middleware.Authentication.AuthenticationOptions>(Configuration.GetSection("AuthenticationOptions"));

builder.Services.Configure<ExceptionLoggingOptions>(Configuration.GetSection("ExceptionLoggingOptions"));
builder.Services.Configure<CachingOptions>(Configuration.GetSection("CachingOptions"));
//builder.Services.Configure<PriceStrategyValidationOptions>(Configuration.GetSection("PriceStrategyValidationOptions"));
builder.Services.Configure<RestrictionTypes>(Configuration.GetSection("RestrictionTypes"));
builder.Services.Configure<OrderServiceOptions>(Configuration.GetSection("OrderServiceOptions"));
//builder.Services.Configure<IndexerOptions>(Configuration.GetSection("IndexerOptions"));

var redisConnection = Configuration.GetValue<string>("Redis.Connection", null);
if (string.IsNullOrWhiteSpace(redisConnection))
{
    var redisHost = Configuration.GetValue<string>("Redis.Host", null);
    var redisPassword = Configuration.GetValue<string>("Redis.Password", null);    
}

//builder.Services.Configure<RbacConfigurationOptions>(rbacOptions =>
//{
//    rbacOptions.RbacAuthorizationEnabled = RbacConfigSettings.Instance.RbacAuthorizationEnabled;
//    rbacOptions.ClaimsAuthorizationEnabled = RbacConfigSettings.Instance.ClaimsAuthorizationEnabled;
//    rbacOptions.RbacOffice365UserAuthorizationEnabled = RbacConfigSettings.Instance.RbacOffice365UserAuthorizationEnabled;
//    rbacOptions.RbacDatabaseUserAuthorizationEnabled = RbacConfigSettings.Instance.RbacDatabaseUserAuthorizationEnabled;
//    rbacOptions.RbacApiKeyUserAuthorizationEnabled = RbacConfigSettings.Instance.RbacApiKeyUserAuthorizationEnabled;
//    rbacOptions.RbacCacheExpirationSeconds = RbacConfigSettings.Instance.RbacCacheExpirationSeconds;
//    rbacOptions.IdentityServiceUrl = Configuration.GetValue<string>("IdentityServiceUrl");
//    rbacOptions.RedisHost = Configuration.GetValue<string>("Redis.Host");
//    rbacOptions.RedisPassword = Configuration.GetValue<string>("Redis.Password");
//    rbacOptions.RedisConnection = redisConnection;
//    rbacOptions.RedisEnabled = Configuration.GetValue<bool>("RedisEnabled", !string.IsNullOrWhiteSpace(redisConnection));
//});

//builder.Services.AddMvc(options =>
//{
   
//    options.Filters.Add<NnaAuthorizationFilter>();
//})
//.AddJsonOptions(options =>
//{
//    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
//});

builder.Services.AddAuthenticationCore(options =>
{
    options.DefaultChallengeScheme = defaultNNASchema;
    options.DefaultForbidScheme = defaultNNASchema;
    options.AddScheme<CustomAuthenticationHandler>(defaultNNASchema, defaultNNASchema);
});

// Logging.
builder.Services.AddTransient<IExceptionLogger, ExceptionLogger>();
builder.Services.AddTransient<LoggingExceptionFilterAttribute>();

// Authentication.
builder.Services.AddTransient<IHmacSha256Hasher, HmacSha256Hasher>();
builder.Services.AddTransient<IHasher, MD5Hasher>();
//builder.Services.AddTransient<IProductHasher, ProductHasher>();
builder.Services.AddTransient<IPackageHasher, PackageHasher>();
builder.Services.AddTransient<IKeySigCredentials, NnaKeySigCredentials>();
builder.Services.AddSingleton<IVaultClient, VaultClient>();
builder.Services.AddSingleton<IApiKeyEncryptionKey, ApiKeyEncryptionKey>();
builder.Services.AddTransient<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddTransient<IApplicationRepository, ApplicationRepository>();
builder.Services.AddTransient<IClaimRepository, ClaimRepository>();
builder.Services.AddTransient<IIdentityProviderRepository, IdentityProviderRepository>();
builder.Services.AddTransient<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddTransient<ILogOnRepository, LogOnRepository>();
builder.Services.AddTransient<ILogOffRepository, LogOffRepository>();
builder.Services.AddTransient<IJObjectValidator, JObjectValidator>();

builder.Services.AddControllers();
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(
                o =>
                o.AddPolicy(
                    "AllowAllCorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    }));

builder.Services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllCorsPolicy");
//app.UseCorrelation();
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
