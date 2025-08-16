using inventory.application.DTOs;
using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int id);
        Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrdersAsync();
        Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersBySupplierAsync(int supplierId);
        Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersByStatusAsync(PurchaseOrderStatus status);
        Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto);
        Task<PurchaseOrderDto> UpdatePurchaseOrderStatusAsync(int id, PurchaseOrderStatus status);
        Task<PurchaseOrderDto> ReceivePurchaseOrderAsync(int id);
        Task DeletePurchaseOrderAsync(int id);
    }
}
