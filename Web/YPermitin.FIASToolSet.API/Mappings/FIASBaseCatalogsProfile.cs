using AutoMapper;

namespace YPermitin.FIASToolSet.API.Mappings
{
    public class FIASBaseCatalogsProfile : Profile
    {
        public FIASBaseCatalogsProfile()
        {
            CreateMap<Models.BaseCatalogs.AddressObjectType, Storage.Core.Models.BaseCatalogs.AddressObjectType>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.ApartmentType, Storage.Core.Models.BaseCatalogs.ApartmentType>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.HouseType, Storage.Core.Models.BaseCatalogs.HouseType>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.NormativeDocKind, Storage.Core.Models.BaseCatalogs.NormativeDocKind>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.NormativeDocType, Storage.Core.Models.BaseCatalogs.NormativeDocType>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.ObjectLevel, Storage.Core.Models.BaseCatalogs.ObjectLevel>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.OperationType, Storage.Core.Models.BaseCatalogs.OperationType>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.ParameterType, Storage.Core.Models.BaseCatalogs.ParameterType>()
                .ReverseMap();
            CreateMap<Models.BaseCatalogs.RoomType, Storage.Core.Models.BaseCatalogs.RoomType>()
                .ReverseMap();
        }
    }
}
