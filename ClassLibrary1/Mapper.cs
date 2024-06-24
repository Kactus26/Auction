using AuctionServer.Model;
using AutoMapper;

namespace Common.DTO
{
    public class Mapper : Profile
    {
        public void MappingProfiles()
        {
            CreateMap<User, UserProfileDTO>();
        }
    }
}
