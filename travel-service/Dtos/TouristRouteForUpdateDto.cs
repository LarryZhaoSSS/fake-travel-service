using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using travel_service.ValidationAttributes;

namespace travel_service.Dtos
{
    public class TouristRouteForUpdateDto : TouristRouteForManipulationDto
    {

        [Required(ErrorMessage = "description不可为空(更新必备)")]
        [MaxLength(1500)]
        public override string Description { get; set; }

    }
}
