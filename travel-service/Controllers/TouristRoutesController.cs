using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using travel_service.Services;
using AutoMapper;
using travel_service.Dtos;
using System.Text.RegularExpressions;
using travel_service.ResourceParameters;
using travel_service.Models;
using Microsoft.AspNetCore.JsonPatch;
using FakeXiecheng.API.Helper;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace travel_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : Controller
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetTouristRoutes([FromQuery] TouristRouteParameters parameters)
        {

            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(parameters.keyword, parameters.ratingOperator, parameters.ratingValue);
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            return Ok(touristRoutesDto);
        }
        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        [HttpHead]
        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"旅游路线{touristRouteId}不存在");
            }
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            return Ok(touristRouteDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreation)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreation);

            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute("GetTouristRouteById", new { touristRouteId = touristRouteModel.Id }, touristRouteToReturn);
        }
        [HttpPut("{touristRouteId}")]
        public async Task<IActionResult> UpdateTouristRoute([FromRoute] Guid touristRouteId,
            [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{touristRouteId}")]
        public async Task<IActionResult> PartiallyUpdateTouristResult([FromRoute] Guid touristRouteId, [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }
        [HttpDelete("{touristRouteId}")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("({touristIds})")]
        public async Task<IActionResult> DeleteByIds([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromQuery] IEnumerable<Guid> touristIds)
        {
            if (touristIds == null)
            {
                return BadRequest();
            }
            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesByIdListAsync(touristIds);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }
    }
}
