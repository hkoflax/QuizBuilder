using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Validations.Quiz
{
    public class StartQuizRequestValidator : AbstractValidator<StartQuizRequest>
    {
        public StartQuizRequestValidator()
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
