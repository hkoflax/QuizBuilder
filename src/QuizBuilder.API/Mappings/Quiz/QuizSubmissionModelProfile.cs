using AutoMapper;
using QuizBuilder.API.DTOs.Quiz.List;
using QuizBuilder.API.DTOs.Quiz.Submit;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;

namespace QuizBuilder.API.Mappings.Quiz
{
    public class QuizSubmissionModelProfile : Profile
    {
        public QuizSubmissionModelProfile()
        {
            CreateMap<QuizSubmissionDto, QuizSubmissionModel>()
                .ForMember(dest => dest.SubmissionId, src => src.MapFrom(x => x.SubmissionId))
                .ForMember(dest => dest.Quiz, src => src.MapFrom(x => x.Quiz))
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(x => x.CreatedAt.ToString(Constants.DefaultDateFormat)))
                .ForMember(dest => dest.Submitedby, src => src.MapFrom(x => x.Submitedby.UserName));

            CreateMap<SelectedResponseDto, SelectedResponseModel>()
                .ForMember(dest => dest.SelectedResponses, src => src.MapFrom(x => x.Answers));

            CreateMap<QuizSubmissionListDto, QuizSubmissionListModel>()
                .ForMember(dest => dest.Submissions, src => src.MapFrom(x => x.Submissions));

            CreateMap<SubmissiondModelBaseDto, SubmissiondModelBase>()
                .ForMember(dest => dest.SubmittedAt, src => src.MapFrom(x => x.SubmittedAt.HasValue ? x.SubmittedAt.Value.ToString(Constants.DefaultDateFormat) : string.Empty));

            CreateMap<SubmitQuizModel, CreateSubmitDto>()
                .ForMember(dest => dest.SelectedResponses, src => src.MapFrom(x => x.SelectedResponses))
                .ReverseMap();

           CreateMap<ICollection<SubmitSelectedResponseModel>, ICollection<CreateSelectedResponseDto>>()
                   .ConvertUsing<Resolvers.SelectedResponseModelResolver>();


        }
    }
}
