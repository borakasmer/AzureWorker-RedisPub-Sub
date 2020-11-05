using AutoMapper;
using Entity.Models.DB;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Exchange, ExchangeModel>();
        CreateMap<ExchangeModel, Exchange>();
    }
}