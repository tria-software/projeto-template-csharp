using ProjetoTemplate.Domain.DTO.Address;
using ProjetoTemplate.Domain.DTO.LayoutLot;
using ProjetoTemplate.Domain.DTO.Profile;
using ProjetoTemplate.Domain.DTO.User;
using ProjetoTemplate.Domain.Helpers;
using ProjetoTemplate.Domain.Models;

namespace ProjetoTemplate.API.Configuration
{
    public class AutoMapperConfig : AutoMapper.Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.FirstAccess, opt => opt.MapFrom(src => src.FirstAccess));

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLowerAndRemoveSpaces()))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Email.ToLowerAndRemoveSpaces()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(x => true))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(x => x.Id == 0 ? DateTimeBrazil.Now() : x.CreateDate))
                .ForMember(dest => dest.LastUpdateDate, opt => opt.MapFrom(x => DateTimeBrazil.Now()));

            CreateMap<AddressDTO, Address>();
            CreateMap<Address, AddressDTO>();

            CreateMap<Profile, ProfileDTO>()
                .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.ProfileAccess.Select(x => x.Module)));

            CreateMap<ProfileDTO, Profile>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(x => true))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(x => x.Id == 0 ? DateTimeBrazil.Now() : x.CreateDate))
                .ForMember(dest => dest.LastUpdateDate, opt => opt.MapFrom(x => DateTimeBrazil.Now()));

            CreateMap<LayoutColumnsDTO, LayoutColumns>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.Columns, opt => opt.MapFrom(x => x.Columns))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(x => DateTimeBrazil.Now()))
                .ForMember(dest => dest.LastUpdateDate, opt => opt.MapFrom(x => DateTimeBrazil.Now()));
        }
    }
}
