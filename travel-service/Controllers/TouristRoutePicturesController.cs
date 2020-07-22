using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using travel_service.Services;
using System.Linq;
using travel_service.Dtos;
using System.Collections.Generic;
using travel_service.Models;
using System.Threading.Tasks;

namespace travel_service.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;
        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _touristRouteRepository = touristRouteRepository ?? throw new ArgumentNullException(nameof(touristRouteRepository));
        }
        [HttpGet]
        public async Task<IActionResult> GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游线不存在");
            }
            var picturesFromRepo = await _touristRouteRepository.GetPicturesByTouristRouteIdAsync(touristRouteId);
            if (picturesFromRepo == null || picturesFromRepo.Count() <= 0)
            {
                return NotFound("没有照片");
            }
            return Ok(_mapper.Map<IEnumerable<TouristRoutePicturesDto>>(picturesFromRepo));
        }
        [HttpGet("{pictureId}", Name = "GetPicture")]
        public async Task<IActionResult> GetPicture(Guid touristRouteId, int pictureId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游线不存在");
            }
            var pictureFromRepo = await _touristRouteRepository.GetPictureAsync(pictureId);
            if (pictureFromRepo == null)
            {
                return NotFound("照片不存在");
            }
            return Ok(_mapper.Map<TouristRoutePicturesDto>(pictureFromRepo));
        }
        [HttpPost]
        public async Task<IActionResult> CreateTouristRoutePicture([FromRoute] Guid touristRouteId, [FromBody] TouristRoutePictureForCreationDto touristRoutePictureForCreationDto)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游线不存在");
            }
            var pictureModel = _mapper.Map<TouristRoutePicture>(touristRoutePictureForCreationDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            await _touristRouteRepository.SaveAsync();
            var pictureToReturn = _mapper.Map<TouristRoutePicturesDto>(pictureModel);
            return CreatedAtRoute("GetPicture", new { touristRouteId = pictureModel.TouristRouteId, pictureId = pictureModel.Id }, pictureToReturn);
        }
        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePictures([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游线不存在");
            }
            var picture = await _touristRouteRepository.GetPictureAsync(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }
    }
}
