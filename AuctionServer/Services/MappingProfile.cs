using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using System.Security.Claims;

namespace AuctionServer.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDTO>()
                .ReverseMap();
        }
    }

}
