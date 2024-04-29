using FluentValidation;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Requests.Validations.Abstractions;

namespace QuizBuilder.Core.Application.Requests.Validations
{
    public class CreateSubmitDtoValidator : BaseValidator<CreateSubmitDto>
    {
        public CreateSubmitDtoValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .WithMessage("Cannot submit an empty answer.");

            RuleFor(c => c.QuizId)
                .NotNull()
                .WithMessage("The 'QuizId' of the quiz you are submitting is required.");

            RuleFor(c => c.SelectedResponses)
                .NotNull()
                .WithMessage("The 'SelectedResponses' of the quiz you are submitting is required.");

            RuleForEach(q => q.SelectedResponses)
                .ChildRules(selection =>
                {
                    selection.RuleFor(c => c.QuestionId)
                             .NotEmpty()
                             .NotNull()
                             .NotEqual(Guid.Empty)
                             .WithMessage("The 'QuestionId' field must be a non-empty Guid.");

                    selection.RuleFor(c => c.AnswerId)
                             .NotEmpty()
                             .NotNull()
                             .NotEqual(Guid.Empty)
                             .WithMessage("The 'AnswerId' field must be a non-empty Guid.");
                });
        }
    }
}
