using AutoMapper;
using QuizBuilder.API.DTOs.Quiz.Create;
using QuizBuilder.API.DTOs.Quiz.List;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;

namespace QuizBuilder.API.Mappings.Quiz
{
    public class QuizModelProfile : Profile
    {
        public QuizModelProfile()
        {
            CreateMap<QuizDto, ListQuizModel>()
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.Author.UserName))
                .ForMember(dest => dest.PublishedDate, src => src.MapFrom(x => x.PublishedDate.HasValue ? x.PublishedDate.Value.ToString(Constants.DefaultDateFormat) : string.Empty));

            CreateMap<QuestionDto, ListQuestionModel>();

            CreateMap<AnswerDto, ListAnswerModel>();

            CreateMap<CreateQuizzModel, QuizDto>()
                .ConstructUsing((src, ctx) => new QuizDto(ctx.Mapper.Map<QuestionDto[]>(src.Questions)));

            CreateMap<CreateQuestionModel, QuestionDto>()
                    .ConstructUsing((src, ctx) => new QuestionDto(ctx.Mapper.Map<AnswerDto[]>(src.Answers)));

            CreateMap<CreateAnswerModel, AnswerDto>();
        }
    }
}