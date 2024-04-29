using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Requests.Validations.Quiz
{
    public class GetQuizSubmissionsValidator : AbstractValidator<GetQuizSubmissions>
    {
        public GetQuizSubmissionsValidator()
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
