using AutoMapper;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;

namespace NZWalksApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() { 
            //Regions Mappings
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<Region, AddRegionRequestDTO>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDTO>().ReverseMap();
        
            //Walks Mappings
            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<Walk, AddWalkRequestDTO>().ReverseMap();
            CreateMap<Walk, UpdateWalkRequestDTO>().ReverseMap();

            //Difficulties Mappings
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
        }
    }
}
