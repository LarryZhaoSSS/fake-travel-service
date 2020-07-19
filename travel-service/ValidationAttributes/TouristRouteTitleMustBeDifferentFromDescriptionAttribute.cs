using System;
using System.ComponentModel.DataAnnotations;
using travel_service.Dtos;

namespace travel_service.ValidationAttributes
{
    public class TouristRouteTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteForManipulationDto)validationContext.ObjectInstance;
            if (touristRouteDto.Title == touristRouteDto.Description)
            {
                return new ValidationResult(
                   "路线名称不能和描述相同",
                   new[] { "TouristRouteForCreationDto" });
            }
            return ValidationResult.Success;
        }
    }
}
