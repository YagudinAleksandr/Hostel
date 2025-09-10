using AutoMapper;
using Hostel.SU.Domain;
using Hostel.Users.Contracts.Response;

namespace Hostel.SU.Application
{
    /// <summary>
    /// Профиль автомаппера для <see cref="User"/>
    /// </summary>
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Name.FirstName))
                .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.Name.Patronymic))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Code))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Code))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
        }
    }
}
