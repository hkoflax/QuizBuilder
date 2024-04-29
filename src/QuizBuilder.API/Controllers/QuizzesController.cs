using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizBuilder.API.Abstractions;
using QuizBuilder.API.Abstractions.Mappings;
using QuizBuilder.API.DTOs.Quiz.Create;
using QuizBuilder.API.DTOs.Quiz.List;
using QuizBuilder.API.DTOs.Quiz.Submit;
using QuizBuilder.API.Settings;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Requests;

namespace QuizBuilder.API.Controllers
{
    public class QuizzesController : QuizAPIControllerBase
    {
        private readonly QuizSettings _questionSettings;

        public QuizzesController(IOptions<QuizSettings> options, IConfiguration configuration) : base()
        {
            _questionSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpGet]
        public async Task<ActionResult> GetUserQuizzes(bool includeQuestionsDetails, [FromServices] IApiModelMapper<ListQuizModel, QuizDto> mapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Queries.GetUserQuizzes(UserId, includeQuestionsDetails);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                return Ok(mapper.MapTo(response.Data));
            }

            return HandleResponse(response.Context);
        }

        [HttpGet("{QuizId}")]
        public async Task<ActionResult> GetQuiz(Guid quizId, [FromServices] IApiModelMapper<ListQuizModel, QuizDto> mapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Queries.GetQuiz(UserId, quizId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(mapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpPost("{QuizId}/publish")]
        public async Task<ActionResult> PublishQuiz(Guid quizId)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Commands.PublishQuiz(UserId, quizId, _questionSettings.MinAllowedQuestionsPerQuiz);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                return NoContent();
            }

            return HandleResponse(response.Context);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuiz(CreateQuizzModel quizzModel, [FromServices] IApiModelMapper<CreateQuizzModel, QuizDto> requestMapper, [FromServices] IApiModelMapper<ListQuizModel, QuizDto> responseMapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = requestMapper.MapFrom(quizzModel);
            var query = Commands.CreateQuiz(dto, UserId, _questionSettings.MaxAllowedQuestionsPerQuiz, _questionSettings.MaxAnswerOptionsPerQuestion);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(responseMapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpPut("{QuizId}")]
        public async Task<ActionResult> UpdateQuiz(Guid quizId, CreateQuizzModel quizzModel, [FromServices] IApiModelMapper<CreateQuizzModel, QuizDto> requestMapper, [FromServices] IApiModelMapper<ListQuizModel, QuizDto> responseMapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = requestMapper.MapFrom(quizzModel);
            var query = Commands.UpdateQuiz(dto, UserId, quizId, _questionSettings.MaxAllowedQuestionsPerQuiz, _questionSettings.MaxAnswerOptionsPerQuestion);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(responseMapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpDelete("{QuizId}")]
        public async Task<ActionResult> DeleteQuiz(Guid quizId)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Commands.DeleteQuiz(UserId, quizId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                return NoContent();
            }

            return HandleResponse(response.Context);
        }

        [HttpPost("{QuizId}/AddQuestion")]
        public async Task<ActionResult> AddQuestionToQuiz(Guid quizId, CreateQuestionModel questionModel, [FromServices] IApiModelMapper<CreateQuestionModel, QuestionDto> requestMapper, [FromServices] IApiModelMapper<ListQuizModel, QuizDto> responseMapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = requestMapper.MapFrom(questionModel);
            var query = Commands.AddQuestionToQuiz(dto, UserId, quizId, _questionSettings.MaxAnswerOptionsPerQuestion);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(responseMapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpPost("{QuizId}/Start")]
        public async Task<ActionResult> StartQuiz(Guid quizId, [FromServices] IApiModelMapper<QuizSubmissionModel, QuizSubmissionDto> responseMapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Commands.StartQuiz(UserId, quizId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(responseMapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpGet("{QuizId}/submissions")]
        public async Task<ActionResult> GetQuizSubmissions(Guid quizId, [FromServices] IApiModelMapper<QuizSubmissionListModel, QuizSubmissionListDto> mapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Queries.GetQuizSubmissions(UserId, quizId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(mapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpGet("my-submissions")]
        public async Task<ActionResult> GetMySubmissions([FromServices] IApiModelMapper<QuizSubmissionModel, QuizSubmissionDto> mapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Queries.GetMySubmissions(UserId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(mapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpGet("Submissions/{submissionId}")]
        public async Task<ActionResult> GetSubmission(Guid submissionId, [FromServices] IApiModelMapper<QuizSubmissionModel, QuizSubmissionDto> responseMapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Queries.GetSubmission(UserId, submissionId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(responseMapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpGet("AvailableQuizzes")]
        public async Task<ActionResult> GetAvailableQuizzes([FromServices] IApiModelMapper<ListQuizModel, QuizDto> mapper, int pageNumber = 1, int pageSize = 1)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Queries.GetAvailableQuizzes(pageNumber, pageSize, UserId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    var data = mapper.MapTo(response.Data);
                    return Ok(new { response.PageNumber, response.PageSize, response.TotalItems, data });
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }

        [HttpPost("{submissionId}/Submit")]
        public async Task<ActionResult> SubmitQuiz(Guid submissionId, SubmitQuizModel submitQuizModel, [FromServices] IApiModelMapper<SubmitQuizModel, CreateSubmitDto> requestMapper, [FromServices] IApiModelMapper<QuizSubmissionModel, QuizSubmissionDto> responseMapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = requestMapper.MapFrom(submitQuizModel);
            var query = Commands.SubmitQuiz(UserId, submissionId, dto);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                if (response.Data != null)
                {
                    return Ok(responseMapper.MapTo(response.Data));
                }
                return NotFound();
            }

            return HandleResponse(response.Context);
        }
    }
}
