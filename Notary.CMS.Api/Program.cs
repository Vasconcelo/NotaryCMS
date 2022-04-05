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

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;

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
//app.UseAuthentication(Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
