using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Queries;

namespace QuizBuilder.Core.Application.Requests.Validations.Quiz
{
    public class GetSubmissionValidator : AbstractValidator<GetSubmission>
    {
        public GetSubmissionValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.SubmissionId)
                .NotEmpty()
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("The 'SubmissionId' field must be a non-empty Guid.");
        }
    }
}
