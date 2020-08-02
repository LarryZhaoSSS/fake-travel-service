using System;
using AutoMapper;
using travel_service.Dtos;
using travel_service.Models;

namespace travel_service.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<LineItem, LineItemDto>();
        }
    }
}
