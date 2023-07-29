using AutoMapper;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;

namespace YPermitin.FIASToolSet.API.Mappings
{
    public class FIASVersionProfile : Profile
    {
        public FIASVersionProfile()
        {
            CreateMap<Models.Versions.FIASVersion, FIASVersion>()
                .ReverseMap();
        }
    }
}
