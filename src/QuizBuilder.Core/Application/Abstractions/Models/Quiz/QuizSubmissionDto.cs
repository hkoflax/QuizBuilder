using QuizBuilder.Core.Application.Abstractions.Models.Account;

namespace QuizBuilder.Core.Application.Abstractions.Models.Quiz
{
    public class QuizSubmissionDto : SubmissiondModelBaseDto, IApplicationModel
    {
        public string QuizTitle { get; set; }
        public bool IsSubmitted { get; set; }
        public QuizDto Quiz { get; set; }
        public UserInfoDto Submitedby { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
