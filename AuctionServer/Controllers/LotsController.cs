using AuctionServer.Interfaces;
using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;

namespace AuctionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotsController : Controller
    {
        private readonly ILotsRepository _lotsRepository;
        private readonly IMapper _mapper;

        public LotsController(ILotsRepository lotsRepository, IMapper mapper)
        {
            _lotsRepository = lotsRepository;
            _mapper = mapper;
        }


        [HttpPost("GetLotOffersInfo")]
        public async Task<IActionResult> GetLotOffersInfo(UserIdDTO userLotIdDTO)
        {
            ICollection<Offer>? offers = await _lotsRepository.GetLotOffersInfo(userLotIdDTO.Id);

            if(offers != null)
            {
                ICollection<OffersDTO> offersDTO = OffersToDTO(offers);
                return Ok(offersDTO);
            }

            return Ok("There is no offers for this lot");
        }

        private ICollection<OffersDTO> OffersToDTO(ICollection<Offer> offers)
        {
            ICollection<OffersDTO> offersDTO = new List<OffersDTO>();

            foreach(Offer offer in offers)
            {
                var offerDTO = new OffersDTO();
                offerDTO.Id = offer.Id;
                offerDTO.Price = offer.Price;
                offerDTO.Name = offer.User.Name;
                offerDTO.Surname = offer.User.Surname;
                offerDTO.DateTime = offer.DateTime;
                offerDTO.Email = offer.User.Email;
                offersDTO.Add(offerDTO);
            }

            return offersDTO;
        }

        [HttpPost("GetLotSellerInfo")]
        public async Task<IActionResult> GetLotSellerInfo(UserIdDTO userLotIdDTO)
        {
            Lot lot = await _lotsRepository.GetLotSeller(userLotIdDTO.Id);
            User owner = lot.Owner;

            if (lot == null)
                NotFound("Lot not found");
            else if (owner == null)
                NotFound("Owner not found");

            byte[] image = System.IO.File.ReadAllBytes(owner.ImageUrl);


            UserProfileDTO ownerDTO = _mapper.Map<UserProfileDTO>(owner);
            UserDataWithImageDTO userData = new UserDataWithImageDTO() { ProfileData = ownerDTO, Image = image };

            return Ok(userData);
        }

        [HttpPost("GetUserLots")]
        public async Task<IActionResult> GetUserLots(PaginationDTO paginationDTO) 
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);

            ICollection<Lot> lots = await _lotsRepository.GetUserLotsByIdWithPagination(userId, paginationDTO.CurrentPage, paginationDTO.PageSize);

            if (lots == null)
                return NotFound("Lots not found");

            List<LotWithImageDTO> lotsWithImage = LotsIntoLotWithImageDTO(lots);

            return Ok(lotsWithImage);
        }

        private List<LotWithImageDTO> LotsIntoLotWithImageDTO(ICollection<Lot> lots)
        {
            List<LotWithImageDTO> lotsWithImages = new List<LotWithImageDTO>();

            foreach (Lot lot in lots)
            {
                LotDTO lotData = _mapper.Map<LotDTO>(lot);
                LotWithImageDTO lotWithImage = new LotWithImageDTO() { LotInfo = lotData };

                if (lot.ImageUrl != null)
                {
                    byte[] image = System.IO.File.ReadAllBytes(lot.ImageUrl);
                    lotWithImage.Image = image;
                }

                lotsWithImages.Add(lotWithImage);
            }
            return lotsWithImages;
        }
    }
}
