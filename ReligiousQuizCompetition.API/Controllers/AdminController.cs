using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReligiousQuizCompetition.Admin.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Admin.Shared.Contracts.Responses;
using ReligiousQuizCompetition.API.DbContexts;
using ReligiousQuizCompetition.API.Models;
using ReligiousQuizCompetition.API.Services;
using ReligiousQuizCompetition.Shared.Contracts.Responses;
using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.Shared.Requests;
using ReligiousQuizCompetition.Shared.Responses;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ReligiousQuizCompetition.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController(AppDbContext dbContext, AuthService authService) : ControllerBase
    {
        private int GetAdminId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                throw new UnauthorizedAccessException();
            return userId;
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] RequestAdminLogin request)
        {
            var userTest = dbContext.AdminUsers.FirstOrDefault(u => u.Username == request.Username);
            if (userTest == null)
                return BadRequest("Admin user is not found.");

            var isAuthenticated = authService.VerifyPassword(request.Password, userTest.Password, userTest.Salt);
            if (!isAuthenticated)
                return Unauthorized("Wrong password");

            var token = authService.GenerateJwtToken(userTest.Id, "admin");
            var refreshToken = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                UserId = userTest.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };

            dbContext.RefreshTokens.Add(refreshToken);
            await dbContext.SaveChangesAsync();

            return Ok(new AdminLoginResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token,
            });
        }

        [HttpPost("auth/refresh")]
        public async Task<IActionResult> RefreshSession([FromBody] RequestRefreshToken request)
        {
            var refreshToken = dbContext.RefreshTokens.FirstOrDefault(x => x.Token == request.Token);
            if (refreshToken == null)
                return Unauthorized();

            if (refreshToken.ExpiresAt <= DateTime.UtcNow
                || refreshToken.IsRevoked)
                return Unauthorized();

            var id = dbContext.AdminUsers
                .Select(u => u.Id)
                .FirstOrDefault(id => id == refreshToken.UserId);
            if (id == 0)
                return Unauthorized();

            var accessToken = authService.GenerateJwtToken(id, "admin");
            var newRefreshToken = new RefreshToken()
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                UserId = id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };

            refreshToken.IsRevoked = true;
            dbContext.RefreshTokens.Update(refreshToken);
            dbContext.RefreshTokens.Add(newRefreshToken);
            await dbContext.SaveChangesAsync();

            return Ok(new RefreshTokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("auth/verify")]
        public async Task<IActionResult> VerifyAdmin()
        {
            var admin = dbContext.AdminUsers.FirstOrDefault(u => u.Id == GetAdminId());
            if (admin == null)
                return NotFound("Admin user was not found.");

            return Ok(true);
        }
    }
}
