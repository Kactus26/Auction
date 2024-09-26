using AuctionServer.Interfaces;
using AuctionServer.Model;
using AuctionServer.Repository;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
