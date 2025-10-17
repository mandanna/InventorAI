using AutoMapper;
using InventorAi_api.EntityModels;
using InventorAi_api.Models;
using InventorAi_api.Models.DTO.Requests.Categories;
namespace InventorAi_api.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LicenseRequest, License>()
                .ForMember(dest => dest.LicenseKey, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddDays(30)))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<License, LicenseServiceResponse>();

            CreateMap<CategoryRequest, Category>();
            CreateMap<CategoryUpdateRequest, Category>();
            CreateMap<Category, CategoryResponse>();


        }
    }
}
