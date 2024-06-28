using AuctionServer.Model;
using AutoMapper;
using CommonDTO;

namespace AuctionServer.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDTO>().ReverseMap();
            CreateMap<User, RegisterUserRequest>().ReverseMap();

        }
    }

}
