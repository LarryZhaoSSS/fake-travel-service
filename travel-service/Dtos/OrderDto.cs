using System;
using System.Collections.Generic;
using travel_service.Models;

namespace travel_service.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
        }
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
        public string State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string TransactionMetadata { get; set; }
    }
}
