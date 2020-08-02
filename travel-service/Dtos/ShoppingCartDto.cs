using System;
using System.Collections.Generic;

namespace travel_service.Dtos
{
    public class ShoppingCartDto
    {

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItemDto> ShoppingCartItems { get; set; }
    }
}
