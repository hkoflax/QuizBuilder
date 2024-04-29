namespace QuizBuilder.API.DTOs.Quiz.Submit
{
    public class SubmitSelectedResponseModel
    {
        public Guid QuestionId { get; set; }
        public ICollection<Guid> SelectedResponses { get; set; } = Array.Empty<Guid>();
    }
}
