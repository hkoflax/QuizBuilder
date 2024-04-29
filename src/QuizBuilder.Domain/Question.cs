namespace QuizBuilder.Domain
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public List<Answer> Answers { get; set; }
        public bool IsMultipleChoice { get; set; }
        public Quiz Quiz { get; set; }
    }
}
