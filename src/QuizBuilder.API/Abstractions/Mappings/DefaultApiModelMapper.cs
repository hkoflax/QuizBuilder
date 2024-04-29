using AutoMapper;
using QuizBuilder.API.Abstractions.Models;
using QuizBuilder.Core.Application.Abstractions.Models;

namespace QuizBuilder.API.Abstractions.Mappings
{
    /// <summary>
    /// A default implementation of <see cref="IApiModelMapper{TApiModel, TApplicationModel}"/> that uses AutoMapper.
    /// </summary>
    /// <typeparam name="TApiModel">The type of <see cref="IApiModel"/>.</typeparam>
    /// <typeparam name="TApplicationModel">The type of <see cref="IApplicationModel"/>.</typeparam>
    public class DefaultApiModelMapper<TApiModel, TApplicationModel> : IApiModelMapper<TApiModel, TApplicationModel>
             where TApiModel : class, IApiModel
             where TApplicationModel : class, IApplicationModel
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApiModelMapper{TApiModel, TApplicationModel}"/> class.
        /// </summary>
        /// <param name="mapper">A <see cref="IMapper"/>.</param>
        public DefaultApiModelMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public TApplicationModel MapFrom(TApiModel source)
            => _mapper.Map<TApplicationModel>(source);

        /// <inheritdoc/>
        public IEnumerable<TApplicationModel> MapFrom(IEnumerable<TApiModel> source)
            => _mapper.Map<TApplicationModel[]>(source);

        /// <inheritdoc/>
        public TApiModel MapTo(TApplicationModel source)
            => _mapper.Map<TApiModel>(source);

        /// <inheritdoc/>
        public IEnumerable<TApiModel> MapTo(IEnumerable<TApplicationModel> source)
            => _mapper.Map<TApiModel[]>(source);
    }
}
