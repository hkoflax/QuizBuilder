namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class SubmissiondModelBaseDto : IApplicationModel
    {
        public Guid SubmissionId { get; set; }
        public double FinalScore { get; set; }
        public DateTime? SubmittedAt { get; set; }

        public ICollection<SelectedResponseDto> SelectedResponses { get; set; } = Array.Empty<SelectedResponseDto>();
    }
}
