using FluentValidation;
using QuizBuilder.Core.Application.Requests.Question;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class DeleteQuestionRequestValidator : AbstractValidator<DeleteQuestionRequest>
    {
        public DeleteQuestionRequestValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.QuestionId)
                .NotEmpty()
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("The 'QuestionId' field must be a non-empty Guid.");
        }
    }
}
