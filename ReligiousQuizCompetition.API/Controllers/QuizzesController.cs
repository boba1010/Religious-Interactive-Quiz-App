using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReligiousQuizCompetition.Admin.Shared.Contracts.Requests;
using ReligiousQuizCompetition.Admin.Shared.Models;
using ReligiousQuizCompetition.API.DbContexts;

namespace ReligiousQuizCompetition.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/admin/[controller]")]
    public class QuizzesController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetQuizzes()
        {
            var quizzes = await dbContext.Quizzes
                .Include(q => q.Questions)
                .AsNoTracking()
                .ToListAsync();

            return Ok(quizzes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetQuiz([FromRoute] int id)
        {
            var quiz = await dbContext.Quizzes
                .Include(q => q.Questions)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                return NotFound();

            return Ok(quiz);
        }

        private List<Models.Question> MapQuestions(List<Question> questions)
        {
            var list = new List<Models.Question>();

            foreach (var question in questions)
            {
                list.Add(new()
                {
                    Answer = question.Answer,
                    Choices = question.Choices,
                    QuestionName = question.QuestionName,
                    QuestionType = (Models.QuestionType)question.QuestionType,
                    Score = question.Score,
                });
            }
            return list;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateQuiz([FromBody] RequestQuizCreation request)
        {
            var newQuiz = new Models.Quiz
            {
                CreationDate = request.CreationDate,
                ExpirationDate = request.ExpirationDate,
                Questions = MapQuestions(request.Questions)
            };

            dbContext.Quizzes.Add(newQuiz);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteQuiz([FromBody] int id)
        {
            var quiz = dbContext.Quizzes.FirstOrDefault(q => q.Id == id);
            if (quiz == null)
                return NotFound();

            dbContext.Quizzes.Remove(quiz);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateQuiz([FromBody] RequestQuizUpdate request)
        {
            var quiz = dbContext.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefault(q => q.Id == request.Id);

            if (quiz == null)
                return NotFound();

            var incomingIds = request.Questions.Select(q => q.Id).ToList();
            var questionsToRemove = quiz.Questions.Where(q => !incomingIds.Contains(q.Id)).ToList();

            foreach (var questionToDelete in questionsToRemove)
            {
                quiz.Questions.Remove(questionToDelete);
            }

            foreach (var incomingDto in request.Questions)
            {
                var existingQuestion = quiz.Questions.FirstOrDefault(q => q.Id == incomingDto.Id && q.Id != 0);

                if (existingQuestion != null)
                {
                    existingQuestion.QuestionName = incomingDto.QuestionName;
                    existingQuestion.Score = incomingDto.Score;
                    existingQuestion.QuestionType = (Models.QuestionType)incomingDto.QuestionType;
                    existingQuestion.Answer = incomingDto.Answer;
                    existingQuestion.Choices = incomingDto.Choices;
                }
                else
                {
                    var newQuestion = new Models.Question
                    {
                        QuestionName = incomingDto.QuestionName,
                        Score = incomingDto.Score,
                        QuestionType = (Models.QuestionType)incomingDto.QuestionType,
                        Answer = incomingDto.Answer,
                        Choices = incomingDto.Choices
                    };
                    quiz.Questions.Add(newQuestion);
                }
            }

            quiz.ExpirationDate = request.ExpirationDate;
            quiz.CreationDate = request.CreationDate;

            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
