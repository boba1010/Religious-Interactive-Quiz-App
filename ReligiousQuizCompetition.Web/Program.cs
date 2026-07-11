using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ReligiousQuizCompetition.Shared.Services;
using ReligiousQuizCompetition.Web;
using ReligiousQuizCompetition.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RanksService>();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISecureStorage, SecureStorage>();
builder.Services.AddScoped<ILocalStorage, LocalStorage>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
