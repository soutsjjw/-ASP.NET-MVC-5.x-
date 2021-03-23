using AutoMapper;
using MessageBoard.Models;

namespace MessageBoard.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ViewModels.Guestbooks.Create, Guestbook>();

            CreateMap<ViewModels.Members.Register, ApplicationUser>()
                .ForMember(x => x.UserName, y => y.MapFrom(s => s.Account));
        }
    }
}
