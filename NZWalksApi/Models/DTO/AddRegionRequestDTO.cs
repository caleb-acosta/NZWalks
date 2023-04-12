﻿using NZWalksApi.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NZWalksApi.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
