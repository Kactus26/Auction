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
            CreateMap<Offer, OffersDTO>().ReverseMap();
            CreateMap<Lot, LotDTO>().ReverseMap();
            CreateMap<Lot, LotChangebleDataDTO>().ReverseMap();
        }
    }
}
