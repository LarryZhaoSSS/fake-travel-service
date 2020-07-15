using System;
namespace travel_service.Dtos
{
    public class TouristRoutePicturesDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid TouristRouteId { get; set; }
    }
}
