using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReligiousQuizCompetition.API.DbContexts;
using ReligiousQuizCompetition.Shared.DTOs;

namespace ReligiousQuizCompetition.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RanksController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRanks()
        {
            var users = context.Users.ToList();

            var leaderboard = users
                                .OrderByDescending(u => u.Score)
                                .Select((entry, index) => new UserRankDTO
                                {
                                    Rank = index + 1,
                                    FullName = entry.FullName,
                                    Score = entry.Score
                                }).ToList();

            return Ok(new UsersRankDTO
            {
                Ranks = leaderboard
            });
        }
    }
}
