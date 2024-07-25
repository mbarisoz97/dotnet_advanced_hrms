using Serilog;
using Ehrms.TrainingManagement.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTrainingManagementApi();
builder.Services.AddDbContext<TrainingDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("TrainingManagementDb");
	options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
});
builder.Services.AddMassTransit(busConfigurator =>
{
	busConfigurator.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("TrainingManagementService", false));
	busConfigurator.AddEventConsumers();
	busConfigurator.UsingRabbitMq((context, configurator) =>
	{
		configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), host =>
		{
			host.Username(builder.Configuration["MessageBroker:Username"]!);
			host.Password(builder.Configuration["MessageBroker:Password"]!);
		});

		configurator.ConfigureEndpoints(context);
	});
});

builder.Host.UseSerilog((context, configuration) =>
{
	configuration.Enrich.FromLogContext()
		.ReadFrom.Configuration(context.Configuration);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var dbInitializer = services.GetRequiredService<TrainingDbSeed>();
	await dbInitializer.SeedAsync();
}

app.Run();

//Do not delete following partial class definition
//It is required for integration tests.
public partial class Program { }