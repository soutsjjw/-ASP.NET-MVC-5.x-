using AutoMapper;
using MessageBoard.Models;

namespace MessageBoard.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ViewModels.Guestbooks.Create, Guestbook>();
        }
    }
}
