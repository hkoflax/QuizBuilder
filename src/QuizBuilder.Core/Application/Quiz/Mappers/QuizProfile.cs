using AutoMapper;
using QuizBuilder.Core.Application.Abstractions.Models.Account;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;

namespace QuizBuilder.Core.Application.Quiz.Mappers
{
    public class QuizProfile: Profile
    {
        public QuizProfile()
        {
            CreateMap<QuizBuilder.Domain.Quiz, QuizDto>()
                .ConstructUsing((src, ctx) => new QuizDto(ctx.Mapper.Map<QuestionDto[]>(src.Questions)))
                .ReverseMap();

            CreateMap<QuizBuilder.Domain.AppUser, UserInfoDto>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.DisplayName))
                .ReverseMap();

            CreateMap<QuizBuilder.Domain.Question, QuestionDto>()
                    .ConstructUsing((src, ctx) => new QuestionDto(ctx.Mapper.Map<AnswerDto[]>(src.Answers)))
                    .ReverseMap();


            CreateMap<QuizBuilder.Domain.Answer, AnswerDto>()
                .ReverseMap();
        }
    }
}
