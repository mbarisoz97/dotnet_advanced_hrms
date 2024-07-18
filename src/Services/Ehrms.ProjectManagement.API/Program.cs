using Ehrms.Shared;
using Serilog;
using Ehrms.ProjectManagement.API.Consumer.EmployeeEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProjectManagementApi();
builder.Services.AddDbContext<ProjectDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("ProjectManagementDb");
	options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
	busConfigurator.AddConsumer<EmployeeCreatedConsumer>();
	busConfigurator.AddConsumer<EmployeeUpdatedConsumer>();
	busConfigurator.AddConsumer<EmployeeDeletedConsumer>();
	busConfigurator.UsingRabbitMq((context, configurator) =>
	{
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
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
	var dbInitializer = services.GetRequiredService<ProjectManagementDatabaseSeed>();
	await dbInitializer.SeedAsync();
}

app.Run();

public partial class Program { }