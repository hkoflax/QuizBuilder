namespace QuizBuilder.Domain
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public double Weight { get; set; }
        public Question Question { get; set; }
    }
}