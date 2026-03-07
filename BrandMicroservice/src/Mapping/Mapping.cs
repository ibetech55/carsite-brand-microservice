using AutoMapper;
using BrandMicroservice.src.Models.DbModels;

namespace BrandMicroservice.src.Mapping
{
    public class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<Make, MakeDto>()
                .ForMember(dest => dest.Models, opt => opt.Ignore());

            CreateMap<Model, ModelDto>()
                .ForMember(dest => dest.Make, opt => opt.MapFrom(src=>src.Makes));
        }
    }
}
