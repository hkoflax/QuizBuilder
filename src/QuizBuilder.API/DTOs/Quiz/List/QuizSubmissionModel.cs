using QuizBuilder.API.Abstractions.Models;

namespace QuizBuilder.API.DTOs.Quiz.List
{
    public class QuizSubmissionModel: SubmissiondModelBase, IApiModel
    {
        public ListQuizModel Quiz{ get; set; }
        public string Submitedby { get; set; }
        public string CreatedAt { get; set; }

    }
}
