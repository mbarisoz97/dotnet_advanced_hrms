using Ehrms.Administration.API;
using Ehrms.Administration.API.Database.Context;
using Ehrms.Administration.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAdministrationApi();

builder.Services.AddDbContext<AdministrationDbContext>(options =>
{
    options.UseInMemoryDatabase("AdministrationDb");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();


//Do not delete following partial class definition.
//It is required for integration tests.
public partial class Program { }