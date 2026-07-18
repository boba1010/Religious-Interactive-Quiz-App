using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReligiousQuizCompetition.API.DbContexts;
using ReligiousQuizCompetition.API.Services;
using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.Contracts.Responses;
using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.API.Models;
using ReligiousQuizCompetition.Shared.Requests;
using ReligiousQuizCompetition.Shared.Responses;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ReligiousQuizCompetition.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(AppDbContext dbContext, AuthService authService) : ControllerBase
    {
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] RequestUserLogin request)
        {
            var userTest = dbContext.Users.FirstOrDefault(u => u.Username == request.Username);
            if (userTest == null)
                return BadRequest("User is not found.");

            var isAuthenticated = authService.VerifyPassword(request.Password, userTest.Password, userTest.Salt);
            if (!isAuthenticated)
                return Unauthorized("Wrong password");

            var token = authService.GenerateJwtToken(userTest.Id, "student");
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

            var userDTO = new UserDTO
            {
                FullName = userTest.FullName,
                Username = userTest.Username,
            };
            return Ok(new UserLoginResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token,
            });
        }

        [HttpPost("auth/signup")]
        public async Task<IActionResult> Signup([FromBody] RequestUserSignup request)
        {
            var userTest = dbContext.Users.FirstOrDefault(u => u.Username == request.Username);
            if (userTest != null)
                return BadRequest("Username is already used.");

            var salt = authService.GenerateSalt();
            var hashedPassword = authService.HashPassword(request.Password, salt);

            User user = new()
            {
                FullName = request.FullName,
                Username = request.Username,
                Password = hashedPassword,
                Salt = salt,
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var savedUser = dbContext.Users.FirstOrDefault(u => u.Username == request.Username);
            var token = authService.GenerateJwtToken(savedUser.Id, "student");
            var refreshToken = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                UserId = savedUser.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };

            dbContext.RefreshTokens.Add(refreshToken);
            await dbContext.SaveChangesAsync();

            var userDTO = new UserDTO
            {
                FullName = user.FullName,
                Username = user.Username,
            };
            return Ok(new UserSignupResponse
            {
                User = userDTO,
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

            var user = dbContext.Users.FirstOrDefault(u => u.Id == refreshToken.UserId);
            if (user == null)
                return Unauthorized();

            var accessToken = authService.GenerateJwtToken(user.Id, "student");
            var newRefreshToken = new RefreshToken()
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                UserId = user.Id,
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

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                throw new UnauthorizedAccessException();
            return userId;
        }

        [Authorize]
        [HttpGet("auth/verify")]
        public async Task<IActionResult> VerifyUser()
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == GetUserId());
            if (user == null)
                return NotFound("User was not found.");

            return Ok(true);
        }
    }
}
