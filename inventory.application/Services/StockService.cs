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
    public class StockService : IStockService
    {
        private readonly IStockTransactionRepository _stockRepo;
        private readonly IProductRepository _productRepo;

        public StockService(
            IStockTransactionRepository stockRepo,
            IProductRepository productRepo)
        {
            _stockRepo = stockRepo;
            _productRepo = productRepo;
        }

        public async Task<StockTransactionDto> CreateStockTransactionAsync(CreateStockTransactionDto dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found");

            if (dto.Type == TransactionType.StockOut && dto.Quantity > product.Quantity)
                throw new ArgumentException("Insufficient stock");

            var transaction = new StockTransaction
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Type = dto.Type,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await _stockRepo.AddAsync(transaction);

            // Update product quantity
            switch (dto.Type)
            {
                case TransactionType.StockIn:
                    product.Quantity += dto.Quantity;
                    break;
                case TransactionType.StockOut:
                    product.Quantity -= dto.Quantity;
                    break;
                case TransactionType.Adjustment:
                    product.Quantity += dto.Quantity;
                    break;
            }

            await _productRepo.UpdateAsync(product);
            return Map(transaction);
        }

        public async Task<IEnumerable<StockTransactionDto>> GetStockTransactionsByProductIdAsync(int productId) =>
            (await _stockRepo.GetByProductIdAsync(productId)).Select(Map);

        public async Task<IEnumerable<StockTransactionDto>> GetAllStockTransactionsAsync() =>
            (await _stockRepo.GetAllAsync()).Select(Map);

        private static StockTransactionDto Map(StockTransaction t) =>
            new(t.Id, t.ProductId, t.Product?.Name, t.Quantity, t.Type, t.Notes, t.CreatedAt);
    }
}
