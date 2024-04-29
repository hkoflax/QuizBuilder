namespace QuizBuilder.Domain
{
    public class SelectedResponse
    {
        public Guid Id { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
        public QuizSubmission QuizSubmission { get; set; }
    }
}
