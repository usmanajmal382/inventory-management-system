using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public class PurchaseOrderService(
     IPurchaseOrderRepository poRepo,
     ISupplierRepository supRepo,
     IProductRepository prodRepo,
     IStockTransactionRepository stockRepo) : IPurchaseOrderService
    {
        public async Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int id) =>
            Map(await poRepo.GetByIdAsync(id));

        public async Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrdersAsync() =>
            (await poRepo.GetAllAsync()).Select(Map);

        public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersBySupplierAsync(int supplierId) =>
            (await poRepo.GetBySupplierAsync(supplierId)).Select(Map);

        public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersByStatusAsync(PurchaseOrderStatus status) =>
            (await poRepo.GetByStatusAsync(status)).Select(Map);

        public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderDto dto)
        {
            if (!await supRepo.ExistsAsync(dto.SupplierId))
                throw new ArgumentException("Supplier does not exist");

            foreach (var i in dto.Items)
                if (!await prodRepo.ExistsAsync(i.ProductId))
                    throw new ArgumentException($"Product {i.ProductId} does not exist");

            var total = dto.Items.Sum(i => i.Quantity * i.UnitPrice);
            var po = new PurchaseOrder
            {
                OrderNumber = await poRepo.GenerateOrderNumberAsync(),
                SupplierId = dto.SupplierId,
                OrderDate = dto.OrderDate,
                DeliveryDate = dto.DeliveryDate,
                Status = PurchaseOrderStatus.Draft,
                TotalAmount = total,
                Notes = dto.Notes,
                Items = dto.Items.Select(i => new PurchaseOrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
            po = await poRepo.AddAsync(po);
            return Map(po);
        }

        public async Task<PurchaseOrderDto> UpdatePurchaseOrderStatusAsync(int id, PurchaseOrderStatus status)
        {
            var po = await poRepo.GetByIdAsync(id) ?? throw new ArgumentException("PO not found");
            po.Status = status;
            await poRepo.UpdateAsync(po);
            return Map(po);
        }

        public async Task<PurchaseOrderDto> ReceivePurchaseOrderAsync(int id)
        {
            var po = await poRepo.GetByIdAsync(id) ?? throw new ArgumentException("PO not found");
            if (po.Status != PurchaseOrderStatus.Approved)
                throw new ArgumentException("Only approved POs can be received");

            foreach (var item in po.Items)
            {
                var tx = new StockTransaction
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Type = TransactionType.PurchaseReceived,
                    Notes = $"Received from PO: {po.OrderNumber}",
                    ReferenceNumber = po.OrderNumber,
                    PurchaseOrderId = po.Id,
                    CreatedAt = DateTime.UtcNow
                };
                await stockRepo.AddAsync(tx);

                var p = await prodRepo.GetByIdAsync(item.ProductId);
                if (p is not null)
                {
                    p.Quantity += item.Quantity;
                    await prodRepo.UpdateAsync(p);
                }
            }

            po.Status = PurchaseOrderStatus.Received;
            po.DeliveryDate = DateTime.UtcNow;
            await poRepo.UpdateAsync(po);
            return Map(po);
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            var po = await poRepo.GetByIdAsync(id) ?? throw new ArgumentException("PO not found");
            if (po.Status == PurchaseOrderStatus.Received)
                throw new ArgumentException("Cannot delete received PO");
            await poRepo.DeleteAsync(id);
        }

        private static PurchaseOrderDto? Map(PurchaseOrder? po) => po is null ? null :
            new(po.Id, po.OrderNumber, po.SupplierId, po.Supplier?.Name, po.OrderDate,
                po.DeliveryDate, po.Status, po.TotalAmount, po.Notes,
                po.Items?.Select(i => new PurchaseOrderItemDto(
                    i.Id, i.ProductId, i.Product?.Name, i.Product?.SKU,
                    i.Quantity, i.UnitPrice, i.TotalPrice)).ToList() ?? new());
    }
}
