using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Validations.Quiz
{
    public class SubmitQuizRequestValidator: AbstractValidator<SubmitQuizRequest>
    {
        public SubmitQuizRequestValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.SubmissionId)
                .NotEmpty()
                .NotNull()
                .NotEqual(Guid.Empty)
                .WithMessage("The 'SubmissionId' field must be a non-empty Guid.");

            RuleFor(c => c.Details)
                 .SetValidator(c => new CreateSubmitDtoValidator());
        }
    }
}
