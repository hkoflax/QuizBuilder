namespace QuizBuilder.API.DTOs.Quiz.List
{
    public class SelectedResponseModel
    {
        public Guid QuestionId { get; set; }
        public string Question { get; set; }
        public ICollection<string> SelectedResponses { get; set; } = Array.Empty<string>();
    }
}
