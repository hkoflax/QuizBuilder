using AutoMapper;
using QuizBuilder.Core.Application.Abstractions.Models.Account;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;
using QuizBuilder.Core.Application.Quiz.Mappers.Resolvers;
using QuizBuilder.Domain;

namespace QuizBuilder.Core.Application.Quiz.Mappers
{
    public class QuizSubmissionProfile : Profile
    {
        public QuizSubmissionProfile()
        {
            CreateMap<ICollection<SelectedResponse>, ICollection<SelectedResponseDto>>()
                     .ConvertUsing<SelectedResponseResolver>();

            CreateMap<QuizSubmission, QuizSubmissionDto>()
                .ForMember(dest => dest.SubmissionId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Submitedby, src => src.MapFrom(x => x.Submittedby))
                .ForMember(dest => dest.SelectedResponses, src => src.MapFrom(x => x.SelectedResponses))
                .ReverseMap();

            CreateMap<AppUser, UserInfoDto>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.DisplayName))
                .ReverseMap();

            CreateMap<QuizSubmission, SubmissiondModelBaseDto>()
                .ForMember(dest => dest.SubmissionId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.FinalScore, src => src.MapFrom(x => x.FinalScore))
                .ForMember(dest => dest.SubmittedAt, src => src.MapFrom(x => x.SubmittedAt))
                .ForMember(dest => dest.SelectedResponses, src => src.MapFrom(x => x.SelectedResponses))
                .ReverseMap();
        }
    }
}
