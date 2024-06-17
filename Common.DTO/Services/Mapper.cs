using AuctionServer.Model;
using AutoMapper;

namespace Common.DTO.Services
{
    public class Mapper : Profile
    {
        public void MappingProfiles()
        {
            CreateMap<User, UserProfileDTO>();
        }
    }
}
