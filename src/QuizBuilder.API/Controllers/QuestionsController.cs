using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizBuilder.API.Abstractions;
using QuizBuilder.API.Abstractions.Mappings;
using QuizBuilder.API.DTOs.Quiz.Create;
using QuizBuilder.API.DTOs.Quiz.List;
using QuizBuilder.API.Settings;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Requests;

namespace QuizBuilder.API.Controllers
{
    public class QuestionsController : QuizAPIControllerBase
    {
        private readonly QuizSettings _questionSettings;

        public QuestionsController(IOptions<QuizSettings> options): base()
        {
            _questionSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpPut("{QuestionId}")]
        public async Task<ActionResult> UpdateQuestion(Guid questionId, CreateQuestionModel questionModel, [FromServices] IApiModelMapper<CreateQuestionModel, QuestionDto> requestMapper, [FromServices] IApiModelMapper<ListQuestionModel, QuestionDto> responseMapper)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = requestMapper.MapFrom(questionModel);
            var query = Commands.UpdateQuestion(dto, UserId, questionId, _questionSettings.MaxAnswerOptionsPerQuestion);
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

        [HttpDelete("{QuestionId}")]
        public async Task<ActionResult> DeleteQuestion(Guid questionId)
        {
            GetAndCheckUserIdAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = Commands.DeleteQuestion(UserId, questionId);
            var response = await Mediator.Send(query);

            if (response.Succeeded)
            {
                return NoContent();
            }

            return HandleResponse(response.Context);
        }
    }
}
