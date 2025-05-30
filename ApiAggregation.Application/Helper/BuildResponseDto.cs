using AutoMapper;

namespace ApiAggregation.Application.Helper;

public class BuildResponseDto
{
    private readonly IMapper _mapper;

    public BuildResponseDto(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    
}