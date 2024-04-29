namespace QuizBuilder.API.DTOs.Quiz.List
{
    public class SubmissiondModelBase
    {
        public Guid SubmissionId { get; set; }
        public double FinalScore { get; set; }
        public string SubmittedAt { get; set; }

        public ICollection<SelectedResponseModel> SelectedResponses { get; set; } = Array.Empty<SelectedResponseModel>();
    }
}
