using AutoMapper;
using inventory.application.Interfaces;
using inventory.application.Services;
using inventory.core.Options;
using inventory.infrastructure.Repositories;
// Add this using directive for SQL Server support
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Services;

namespace inventory.infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                options.UseSqlServer(provider.GetRequiredService<IOptionsSnapshot<ConnectionStringOptions>>().Value.DefaultConnection);
            });
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IStockAlertRepository, StockAlertRepository>();
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IStockAlertService, StockAlertService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitService, UnitService>();
       
            services.AddScoped<ISalesService, SalesService>();

            return services;
        }
    }
}