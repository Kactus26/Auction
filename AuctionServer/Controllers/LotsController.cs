using AuctionServer.Interfaces;
using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

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

        [HttpPost("CloseLot")]
        public async Task<IActionResult> CloseLot(UserIdDTO lotIdDTO)
        {
            int userId = Convert.ToInt32(User.Identities.First().Claims.First().Value);
            Lot lot = await _lotsRepository.GetLotWithSeller(lotIdDTO.Id);

            if (lot == null)
                return NotFound("Lot not found");
            else if (lot.Owner.Id != userId)
                return BadRequest("User in not owner");

            lot.IsClosed = true;
            await _lotsRepository.SaveChanges();

            return Ok("Lot closed!");
        }

        [HttpPost("SendOffer")]
        public async Task<IActionResult> SendOffer(OfferPrice offerPrice)
        {
            int userId = Convert.ToInt32(User.Identities.First().Claims.First().Value);

            User userWhoOffers = await _lotsRepository.GetUserById(userId);

            Lot lot = await _lotsRepository.GetLotById(offerPrice.LotId);

            if (userWhoOffers == null || lot == null)
                return NotFound("User or lot not found at SendOffer method");

            var result = await _lotsRepository.AddOffer( new Offer() { DateTime = DateTime.Now, Price = offerPrice.Price, User = userWhoOffers, Lot = lot } );

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                await _lotsRepository.SaveChanges();
                return Ok("Offer added successfully");
            }
            return BadRequest("Something went wrong");
        }


        [HttpPost("GetLotAndOffersInfo")]
        public async Task<IActionResult> GetLotAndOffersInfo(UserIdDTO userLotIdDTO)
        {
            Lot lot = await _lotsRepository.GetLotById(userLotIdDTO.Id);

            if (lot == null)
                return NotFound("Lot not found");

            ICollection<Offer>? offers = await _lotsRepository.GetLotOffersInfo(userLotIdDTO.Id);

            LotWithOfferDTO lotWithOfferDTO = new LotWithOfferDTO() { Offers = OffersToDTO(offers), LotInfo = _mapper.Map<LotChangebleDataDTO>(lot) };
            
            return Ok(lotWithOfferDTO);
        }

        private ICollection<OffersDTO> OffersToDTO(ICollection<Offer> offers)
        {
            ICollection<OffersDTO> offersDTO = new List<OffersDTO>();

            foreach(Offer offer in offers)
            {
                var offerDTO = _mapper.Map<OffersDTO>(offer);
                offersDTO.Add(offerDTO);
            }

            return offersDTO;
        }

        [HttpPost("GetLotSellerInfo")]
        public async Task<IActionResult> GetLotSellerInfo(UserIdDTO userLotIdDTO)
        {
            Lot lot = await _lotsRepository.GetLotWithSeller(userLotIdDTO.Id);
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
