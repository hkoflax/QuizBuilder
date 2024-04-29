using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class DeleteQuizRequestValidator : AbstractValidator<DeleteQuizRequest>
    {
        public DeleteQuizRequestValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.QuizId)
                .NotEmpty()
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("The 'QuizId' field must be a non-empty Guid.");
        }
    }
}
