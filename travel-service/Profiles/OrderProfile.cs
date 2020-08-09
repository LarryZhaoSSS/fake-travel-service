using System;
using AutoMapper;
using travel_service.Dtos;
using travel_service.Models;

namespace travel_service.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                    dest => dest.State,
                    opt =>
                    {
                        opt.MapFrom(src => src.State.ToString());
                    }
                    );

        }
    }
}
