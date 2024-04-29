namespace QuizBuilder.API.DTOs.Quiz.Abstractions
{
    public abstract class QuestionModelBase
    {
        public string Text { get; set; }
        public bool IsMultipleChoice { get; set; }
    }
}
