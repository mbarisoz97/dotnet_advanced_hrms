using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.API.Database.Models;
using Microsoft.EntityFrameworkCore;
using Ehrms.Shared;
using Ehrms.Authentication.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationApi();
builder.Services.AddIdentity<User, Role>()
	.AddEntityFrameworkStores<ApplicationUserDbContext>();

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

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var dbInitializer = services.GetRequiredService<ApplicationUserDbSeed>();
	await dbInitializer.SeedAsync();
}

app.Run();

//Do not remove following partial class definition.
//It is required for integration tests.
public partial class Program { }