using inventory.application.Services;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.NotificationPublisher = new TaskWhenAllPublisher();
            });
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IStockAlertService, StockAlertService>();
            services.AddScoped<IStockService, StockService>();


            return services;
        }
    }
}
