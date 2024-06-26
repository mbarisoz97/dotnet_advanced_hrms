using Ehrms.Shared;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddCustomJwtAuthentication();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapReverseProxy();

app.Run();