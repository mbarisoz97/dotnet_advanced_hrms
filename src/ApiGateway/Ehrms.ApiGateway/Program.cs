using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
    //.ConfigureHttpClient((context, handler) =>
    //{
    //    handler.AllowAutoRedirect = true;
    //});

var app = builder.Build();

app.MapReverseProxy();

app.Run();