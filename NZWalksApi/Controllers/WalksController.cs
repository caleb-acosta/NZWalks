using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksApi.CustomActionFilters;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;
using NZWalksApi.Repositories;

namespace NZWalksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository) {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO walkRequest) {
            Walk domainWalk = mapper.Map<Walk>(walkRequest);
            await walkRepository.CreateAsync(domainWalk);
            return Ok(mapper.Map<WalkDTO>(domainWalk));

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize) {
            return Ok(mapper.Map<List<WalkDTO>>(
                await walkRepository.GetAllAsync(
                    filterOn, 
                    filterQuery, 
                    sortBy, 
                    isAscending ?? true,
                    pageNumber,
                    pageSize))
                );
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            Walk? walkDomain = await walkRepository.GetByIdAsync(id);
            return walkDomain is null ? NotFound() : Ok(mapper.Map<WalkDTO>(walkDomain));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO walkRequest) {
            Walk? walkDomain = await walkRepository.UpdateAsync(id, mapper.Map<Walk>(walkRequest));

            return walkDomain is null ? NotFound() : Ok(mapper.Map<WalkDTO>(walkDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            Walk? walkDomain = await walkRepository.DeleteAsync(id);
            return walkDomain is null ? NotFound() : Ok(mapper.Map<WalkDTO>(walkDomain));
        }
    }
}
