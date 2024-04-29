namespace QuizBuilder.Core.Application.Abstractions.Models.Account
{
    public class UserInfoDto: IApplicationModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
}
