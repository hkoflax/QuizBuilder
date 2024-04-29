using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class GetQuizValidator : AbstractValidator<GetQuiz>
    {
        public GetQuizValidator()
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
