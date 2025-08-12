using inventory.core.Entities;
using MyApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Interfaces
{
    // IOrderRepository.cs
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderWithItemsAsync(int id);
    }
}
