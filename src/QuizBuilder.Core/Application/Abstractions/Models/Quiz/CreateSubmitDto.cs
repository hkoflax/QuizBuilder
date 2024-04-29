namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class CreateSubmitDto : IApplicationModel
    {
        public Guid QuizId { get; set; }

        public ICollection<CreateSelectedResponseDto> SelectedResponses { get; set; } = Array.Empty<CreateSelectedResponseDto>();
    }
}
