using FluentValidation;
using QuizBuilder.Core.Application.Requests.Quiz.Commands;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class AddQuestionToQuizRequestValidator : AbstractValidator<AddQuestionToQuizRequest>
    {
        public AddQuestionToQuizRequestValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.QuizId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.Details)
                .NotNull()
                .WithMessage("Cannot add an empty question.");

            RuleFor(c => c.Details)
                .SetValidator(c => new QuestionDtoValidator(c.MaxAnswerOptionsPerQuestion));
        }
    }
}
