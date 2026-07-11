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
        policy.WithOrigins("https://religious-interactive-quiz.onrender.com")
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
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

List<Question> questions = 
[
    new()
    {
        Answer = "2",
        QuestionName = "1 + 1 = ?",
        QuestionType = QuestionType.EssayQuestion
    },
    new()
    {
        Answer = "5",
        QuestionName = "2 + 3 = ?",
        QuestionType = QuestionType.EssayQuestion
    },
];
Quiz quiz = new()
{
    Questions = questions,
    CreationDate = DateTime.UtcNow,
    ExpirationDate = DateTime.UtcNow.AddHours(12)
};

context.Quizzes.Add(quiz);
await context.SaveChangesAsync();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("BlazorWasmPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
