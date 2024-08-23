using Ehrms.Authentication.API.Database.Context;
using Microsoft.EntityFrameworkCore;
using Ehrms.Shared;
using Ehrms.Authentication.API.Middleware;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationApi();
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationUserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationUserDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("AuthDb");
    options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
});
builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

await app.CheckDatabase();
await app.AddUserRoles();
if (app.Environment.IsDevelopment())
{
    await app.SeedDatabase();
}

app.Run();

//Do not remove following partial class definition.
//It is required for integration tests.
public partial class Program { }