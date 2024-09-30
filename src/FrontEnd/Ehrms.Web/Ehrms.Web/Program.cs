using Ehrms.Web;
using Ehrms.Web.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using BitzArt.Blazor.Cookies;
using Blazored.Toast;
using Blazored.Modal;
using MudBlazor.Services;
using Ehrms.Web.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebUi();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddMudServices();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.Name = "auth_token";
		options.LoginPath = UserRouting.Login;
		options.AccessDeniedPath = ErrorRouting.AccessError;
		options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
	});
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.AddBlazorCookies();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(HttpClients.BackendApiGateway, client =>
{
	client.BaseAddress = new Uri(builder.Configuration["ApiGatewayUri"]!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(Ehrms.Web.Client._Imports).Assembly);

app.Run();
