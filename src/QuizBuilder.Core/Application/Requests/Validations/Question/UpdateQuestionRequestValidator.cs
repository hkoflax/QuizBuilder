using FluentValidation;
using QuizBuilder.Core.Application.Requests.Question;
using QuizBuilder.Core.Application.Requests.Validations.Abstractions;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class UpdateQuestionRequestValidator : BaseValidator<UpdateQuestionRequest>
    {
        public UpdateQuestionRequestValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(c => c.QuestionId)
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
