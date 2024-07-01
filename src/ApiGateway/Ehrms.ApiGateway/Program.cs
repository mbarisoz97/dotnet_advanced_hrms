using Serilog;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddCustomJwtAuthentication();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Host.UseSerilog((context, configuration) =>
{
	configuration.Enrich.FromLogContext()
		.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

app.UseSerilogRequestLogging();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapReverseProxy();

app.Run();