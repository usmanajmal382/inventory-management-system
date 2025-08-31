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
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleItem> SaleItems => Set<SaleItem>();

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
                // ... existing configuration ...
                e.HasOne(p => p.Unit).WithMany(u => u.Products).HasForeignKey(p => p.UnitId).IsRequired(false);
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
            // Order
            b.Entity<Order>(e =>
            {
                e.HasKey(o => o.Id);
                e.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
                e.HasIndex(o => o.OrderNumber).IsUnique();
                e.Property(o => o.TotalAmount).HasPrecision(18, 2);
                e.Property(o => o.TaxAmount).HasPrecision(18, 2);
                e.Property(o => o.DiscountAmount).HasPrecision(18, 2);
            });

            // OrderItem
            b.Entity<OrderItem>(e =>
            {
                e.HasKey(i => i.Id);
                e.Property(i => i.UnitPrice).HasPrecision(18, 2);
                e.Property(i => i.Discount).HasPrecision(18, 2);
                e.Property(i => i.Unit).HasMaxLength(50);
                e.HasOne(i => i.Order).WithMany(o => o.Items).HasForeignKey(i => i.OrderId);
                e.HasOne(i => i.Product).WithMany(p => p.OrderItems).HasForeignKey(i => i.ProductId);
            });

            // Unit
            b.Entity<Unit>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.Name).IsRequired().HasMaxLength(20);
                e.Property(u => u.Abbreviation).HasMaxLength(10);
                e.HasIndex(u => u.Name).IsUnique();
            });
            // Sale
            b.Entity<Sale>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
                e.HasIndex(s => s.SaleNumber).IsUnique();
                e.Property(s => s.TotalAmount).HasPrecision(18, 2);
                e.Property(s => s.TaxAmount).HasPrecision(18, 2);
                e.Property(s => s.DiscountAmount).HasPrecision(18, 2);
                e.HasOne(s => s.Order).WithMany().HasForeignKey(s => s.OrderId).IsRequired(false);
            });

            // SaleItem
            b.Entity<SaleItem>(e =>
            {
                e.HasKey(si => si.Id);
                e.Property(si => si.UnitPrice).HasPrecision(18, 2);
                e.Property(si => si.Unit).HasMaxLength(50);
                e.HasOne(si => si.Sale).WithMany(s => s.Items).HasForeignKey(si => si.SaleId);
                e.HasOne(si => si.Product).WithMany(p => p.SaleItems).HasForeignKey(si => si.ProductId);
            });

        }
    }
}
