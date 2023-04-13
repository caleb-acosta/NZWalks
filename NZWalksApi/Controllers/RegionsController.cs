using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NZWalksApi.CustomActionFilters;
using NZWalksApi.Data;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;
using NZWalksApi.Repositories;
using System.Text.Json;

namespace NZWalksApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(
            IRegionRepository regionRepository, 
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll() {
            try
            {
                //throw new Exception("Custom exception");
                List<Region> regionsDomain = await regionRepository.GetAllAsync();
                logger.LogInformation($"All Regions: {JsonSerializer.Serialize(regionsDomain)}");
                return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
            }
            catch (Exception ex) {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Region? region = await regionRepository.GetByIdAsync(id);
            return region is null ? NotFound() : Ok(mapper.Map<RegionDTO>(region));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO regionRequest) {
            Region regionDomain = mapper.Map<Region>(regionRequest);
            await regionRepository.CreateAsync(regionDomain);
            return CreatedAtAction(nameof(GetById), new { regionDomain.Id }, mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO regionRequest) {

            Region? regionDomain = mapper.Map<Region>(regionRequest);
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);

            return regionDomain == null? NotFound() : Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Writer")]

        public async Task <IActionResult> Delete([FromRoute] Guid id) {
            Region? regionDomain = await regionRepository.DeleteAsync(id);
            return regionDomain is null ? NotFound() : Ok(mapper.Map<RegionDTO>(regionDomain));
        }
    }
}
