using AutoMapper;

namespace YPermitin.FIASToolSet.API.Mappings
{
    public class FIASVersionProfile : Profile
    {
        public FIASVersionProfile()
        {
            CreateMap<Models.FIASVersion, Storage.Core.Models.FIASVersion>()
                .ReverseMap();
        }
    }
}
