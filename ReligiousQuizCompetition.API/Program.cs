using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReligiousQuizCompetition.API.DbContexts;
using ReligiousQuizCompetition.API.Models;
using ReligiousQuizCompetition.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<AuthService>();


var aivenConnectionString = Environment.GetEnvironmentVariable("AIVEN_POSTGRES_CONN");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(aivenConnectionString))
    {
        options.UseNpgsql(aivenConnectionString);
    }
    else
    {
        options.UseInMemoryDatabase("AppDb");
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorWasmPolicy", policy =>
    {
        policy.WithOrigins(
            "https://localhost:7150",
            "https://religious-interactive-quiz.onrender.com",
            "https://localhost:7284",
            "https://religious-interactive-quiz-app.onrender.com"
            )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(Environment.GetEnvironmentVariable("JWT_KEY")!)
        ),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"FAIL: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});



var app = builder.Build();

using var scope = app.Services.CreateScope();
try
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}
catch { }

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var adminUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
var adminHashedPassword = Environment.GetEnvironmentVariable("ADMIN_HASHED_PASSWORD");
var adminSaltRawString = Environment.GetEnvironmentVariable("ADMIN_SALT");

if (!string.IsNullOrEmpty(adminUsername) &&
    !string.IsNullOrEmpty(adminHashedPassword) &&
    !string.IsNullOrEmpty(adminSaltRawString))
{
    byte[] adminSaltBytes = [.. adminSaltRawString
        .Replace("[", "")
        .Replace("]", "")
        .Replace(" ", "")
        .Split(',')
        .Select(byte.Parse)];

    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var adminExists = await dbContext.AdminUsers.AnyAsync(a => a.Username == adminUsername);
    if (!adminExists)
    {
        var newAdmin = new AdminUser
        {
            Username = adminUsername,
            Password = adminHashedPassword,
            Salt = adminSaltBytes
        };

        dbContext.AdminUsers.Add(newAdmin);
        await dbContext.SaveChangesAsync();

        Console.WriteLine("Admin account seeded successfully into the database.");
    }
}
else
    throw new Exception("ADMIN USER DATA IS MISSING!");

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("BlazorWasmPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
