using Microsoft.EntityFrameworkCore;
using ReligiousQuizCompetition.API.Models;

namespace ReligiousQuizCompetition.API.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
