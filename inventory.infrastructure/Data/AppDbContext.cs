using inventory.core.Entities;
using inventory.core.Models;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
        public DbSet<StockAlert> StockAlerts => Set<StockAlert>();
        public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
        public DbSet<ProductSupplier> ProductSuppliers => Set<ProductSupplier>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Product
            b.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).IsRequired().HasMaxLength(100);
                e.Property(p => p.SKU).IsRequired().HasMaxLength(50);
                e.HasIndex(p => p.SKU).IsUnique();
                e.Property(p => p.Price).HasPrecision(18, 2);
                e.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
            });

            // Category
            b.Entity<Category>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // Supplier
            b.Entity<Supplier>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.Name).IsRequired().HasMaxLength(200);
            });

            // PurchaseOrder
            b.Entity<PurchaseOrder>(e =>
            {
                e.HasKey(po => po.Id);
                e.Property(po => po.OrderNumber).IsRequired().HasMaxLength(50);
                e.HasIndex(po => po.OrderNumber).IsUnique();
                e.Property(po => po.TotalAmount).HasPrecision(18, 2);
                e.HasOne(po => po.Supplier).WithMany(s => s.PurchaseOrders).HasForeignKey(po => po.SupplierId);
            });

            // PurchaseOrderItem
            b.Entity<PurchaseOrderItem>(e =>
            {
                e.HasKey(i => i.Id);
                e.Property(i => i.UnitPrice).HasPrecision(18, 2);
                e.HasOne(i => i.PurchaseOrder).WithMany(po => po.Items).HasForeignKey(i => i.PurchaseOrderId);
                e.HasOne(i => i.Product).WithMany(p => p.PurchaseOrderItems).HasForeignKey(i => i.ProductId);
            });

            // ProductSupplier (many-to-many bridge)
            b.Entity<ProductSupplier>(e =>
            {
                e.HasKey(ps => new { ps.ProductId, ps.SupplierId });
                e.Property(ps => ps.SupplierPrice).HasPrecision(18, 2);
                e.HasOne(ps => ps.Product).WithMany(p => p.ProductSuppliers).HasForeignKey(ps => ps.ProductId);
                e.HasOne(ps => ps.Supplier).WithMany(s => s.ProductSuppliers).HasForeignKey(ps => ps.SupplierId);
            });

            // StockAlert
            b.Entity<StockAlert>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Message).HasMaxLength(500);
                e.HasOne(a => a.Product).WithMany(p => p.StockAlerts).HasForeignKey(a => a.ProductId);
            });

            // StockTransaction
            b.Entity<StockTransaction>(e =>
            {
                e.HasKey(t => t.Id);
                e.Property(t => t.Notes).HasMaxLength(500);
                e.Property(t => t.ReferenceNumber).HasMaxLength(100);
                e.HasOne(t => t.Product).WithMany(p => p.StockTransactions).HasForeignKey(t => t.ProductId);
                e.HasOne(t => t.PurchaseOrder).WithMany().HasForeignKey(t => t.PurchaseOrderId).IsRequired(false);
            });
        }
    }
}
