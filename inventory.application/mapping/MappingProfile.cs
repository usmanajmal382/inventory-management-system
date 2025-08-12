using AutoMapper;
using inventory.application.DTOs;
using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product
            CreateMap<ProductDto, Product>().ReverseMap();
            // Category
            CreateMap<CategoryDto, Category>().ReverseMap();
            // Order
            CreateMap<OrderDto, Order>().ReverseMap();
            // OrderItem
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        }
    }

}
