namespace QuizBuilder.Domain
{
    public class QuizSubmission
    {
        public Guid Id { get; set; }
        public string QuizTitle { get; set; }
        public double FinalScore { get; set; }
        public bool IsSubmitted { get; set; }

        public ICollection<SelectedResponse> SelectedResponses { get; set; } 
        public Quiz Quiz { get; set; }
        public AppUser Submittedby { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SubmittedAt { get; set;}
    }
}
