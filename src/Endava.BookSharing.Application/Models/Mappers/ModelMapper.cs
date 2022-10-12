using AutoMapper;
using Endava.BookSharing.Application.Abstract;

namespace Endava.BookSharing.Application.Models.Mappers;

public class ModelMapper : IModelMapper
{
    private readonly IMapper _mapper;

    public ModelMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TResult Map<TResult>(object source)
    {
        return _mapper.Map<TResult>(source);
    }
}