namespace inventory.application.Interfaces
{
    // IOrderRepository.cs
    public interface IReportingService
    {
        Task<InventoryValueReport> GetInventoryValueReportAsync();
        Task<IEnumerable<TopSellingProductReport>> GetTopSellingProductsAsync(DateTime from, DateTime to, int count = 10);
        Task<IEnumerable<StockMovementReport>> GetStockMovementReportAsync(DateTime from, DateTime to);
        Task<SupplierPerformanceReport> GetSupplierPerformanceAsync(int supplierId, DateTime from, DateTime to);
    }

    public record InventoryValueReport(decimal TotalInventoryValue, int TotalProducts, int LowStockProducts, int OutOfStockProducts, decimal AverageStockValue);
    public record TopSellingProductReport(int ProductId, string ProductName, string SKU, int TotalQuantitySold, decimal TotalRevenue);
    public record StockMovementReport(int ProductId, string ProductName, int StockIn, int StockOut, int NetMovement);
    public record SupplierPerformanceReport(int SupplierId, string SupplierName, int TotalOrders, int CompletedOrders, decimal AverageDeliveryTime, decimal TotalOrderValue);
}
