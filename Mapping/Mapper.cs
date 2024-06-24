using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using System.Security.Claims;

namespace Mapping
{
    public class Mapper : Profile, IMappingProfile
    {
        public Mapper()
        {
            ConfigureMapping(this);
        }

        public void ConfigureMapping(Profile profile)
        {
            CreateMap<User, UserProfileDTO>().ReverseMap();
            CreateMap<Lot, LotDTO>().ReverseMap();

        }

    }
}
