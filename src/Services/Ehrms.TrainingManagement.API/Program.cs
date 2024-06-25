using Ehrms.TrainingManagement.API.Consumers.EmployeeEvent;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTrainingManagementApi();
builder.Services.AddDbContext<TrainingDbContext>(config =>
{
	config.UseInMemoryDatabase("TrainingDb");
});
builder.Services.AddMassTransit(busConfigurator =>
{
	busConfigurator.SetKebabCaseEndpointNameFormatter();
	busConfigurator.AddConsumer<EmployeeCreatedEventConsumer>();
	busConfigurator.AddConsumer<EmployeeDeletedEventConsumer>();
	busConfigurator.AddConsumer<EmployeeUpdatedEventConsumer>();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
