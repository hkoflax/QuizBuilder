using AutoMapper;
using QuizBuilder.API.DTOs.Quiz.Submit;
using QuizBuilder.Core.Application.Abstractions.Models.Quiz;

namespace QuizBuilder.API.Mappings.Resolvers
{
    public class SelectedResponseModelResolver : ITypeConverter<ICollection<SubmitSelectedResponseModel>, ICollection<CreateSelectedResponseDto>>
    {
        public ICollection<CreateSelectedResponseDto> Convert(ICollection<SubmitSelectedResponseModel> source, ICollection<CreateSelectedResponseDto> destination, ResolutionContext context)
        {
            if (source == null) return Array.Empty<CreateSelectedResponseDto>();

            var flattenedResponses = source.SelectMany(sr => sr.SelectedResponses.Select(answer =>
                new CreateSelectedResponseDto
                {
                    QuestionId = sr.QuestionId,
                    AnswerId = answer
                }))
                .ToList();

            return flattenedResponses;
        }
    }
}
