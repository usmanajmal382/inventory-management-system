using AutoMapper;
using inventory.application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Services;
// Add this using directive for SQL Server support
using Microsoft.EntityFrameworkCore;
using inventory.core.Options;
using inventory.infrastructure.Repositories;

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
            return services;
        }
    }
}