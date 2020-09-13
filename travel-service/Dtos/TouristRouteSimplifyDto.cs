using System;
namespace travel_service.Dtos
{
    public class TouristRouteSimplifyDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public TouristRouteSimplifyDto()
        {
        }
    }
}
