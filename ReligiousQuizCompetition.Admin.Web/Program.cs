using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ReligiousQuizCompetition.Admin.Web;
using ReligiousQuizCompetition.Admin.Web.Services;
using ReligiousQuizCompetition.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ReligiousQuizCompetition.Admin.Web.Services.AuthService>();
builder.Services.AddScoped<ReligiousQuizCompetition.Admin.Web.Services.ApiClient>();
builder.Services.AddScoped<ReligiousQuizCompetition.Admin.Web.Services.QuizService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISecureStorage, SecureStorage>();
builder.Services.AddScoped<ILocalStorage, LocalStorage>();

await builder.Build().RunAsync();
