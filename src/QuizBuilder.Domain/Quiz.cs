namespace QuizBuilder.Domain
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public List<Question> Questions { get; set; }
        public List<QuizSubmission> QuizSubmissions { get; set; }
        public AppUser Author { get; set; }
    }
}
