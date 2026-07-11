using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReligiousQuizCompetition.API.DbContexts;
using ReligiousQuizCompetition.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Shared.Contracts.Responses;
using ReligiousQuizCompetition.Shared.DTOs;
using ReligiousQuizCompetition.Shared.Models;
using System.Security.Claims;

namespace ReligiousQuizCompetition.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/quiz")]
    public class QuizController(AppDbContext dbContext) : ControllerBase
    {
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                throw new UnauthorizedAccessException();
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuiz()
        {
            var quiz = dbContext.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefault();

            if (quiz == null)
                return Ok("There are no quizzes for today.");

            var quizDTO = new QuizDTO()
            {
                Id = quiz.Id,
                Questions = [],
            };

            foreach (var question in quiz.Questions)
            {
                quizDTO.Questions.Add(new()
                {
                    Id = question.Id,
                    Choices = question.Choices,
                    QuestionName = question.QuestionName,
                    QuestionType = question.QuestionType
                });
            }

            return Ok(quizDTO);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitQuiz([FromBody] SubmitQuizRequest request)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == GetUserId());
            if (user == null)
                return NotFound("User is not found.");

            var quiz = dbContext.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefault(q => q.Id == request.Id);
            if (quiz == null)
                return NotFound("Quiz cannot be found.");

            List<Question> questions = [];

            foreach (var question in quiz.Questions)
                questions.Add(question);

            int correctAnswers = 0;

            foreach(var answer in request.Answers)
            {
                foreach (var q in questions)
                {
                    if (q.Answer == answer)
                        correctAnswers++;
                }
            }

            user.Score += correctAnswers;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();

            return Ok(new SubmitQuizResponse
            {
                CorrectAnswersCount = correctAnswers
            });
        }
    }
}
