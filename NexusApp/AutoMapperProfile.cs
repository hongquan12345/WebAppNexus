using AutoMapper;
using NexusApp.Areas.Customer.Models;
using NexusApp.ModelDTOs;

namespace NexusApp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerModel, LoginDTOs>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<CustomerModel, UserDetailDTOs>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.BirthDay))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.Ward))
                 .ForMember(dest => dest.statusAccount, opt => opt.MapFrom(src => src.Accounts.Status))
                 .ForMember(dest => dest.AccoutCode, opt => opt.MapFrom(src => src.Accounts.AccountCode))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
            CreateMap<CustomerModel, ChangePasswordDTOs>();
            CreateMap<CustomerModel, UpdateCustomerDTO>()
                      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId));
        }
    }
}
