using Microsoft.EntityFrameworkCore;
using PETFactoryERP.Models;

namespace PETFactoryERP.DAL
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;
        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<RawMaterial> RawMaterials { get; set; } = null!;
        public DbSet<ProductionOrder> ProductionOrders { get; set; } = null!;
        public DbSet<FinishedStock> FinishedStock { get; set; } = null!;
        public DbSet<RawMaterialTransaction> RawMaterialTransactions { get; set; } = null!;
        public DbSet<SalesInvoice> SalesInvoices { get; set; } = null!;
        public DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; set; } = null!;
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; } = null!;
        public DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; } = null!;
        public DbSet<DailyWage> DailyWages { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<CashTransaction> CashTransactions { get; set; } = null!;
        public DbSet<ProductRawMaterial> ProductRawMaterials { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(eb =>
            {
                eb.Property(p => p.Area_m2).HasPrecision(18,6);
                eb.Property(p => p.Thickness_mm).HasPrecision(18,6);
                eb.Property(p => p.Density_g_cm3).HasPrecision(18,6);
            });

            modelBuilder.Entity<RawMaterial>(eb =>
            {
                eb.Property(r => r.StockQtyKg).HasPrecision(18,4);
                eb.Property(r => r.ReorderLevelKg).HasPrecision(18,4);
                eb.Property(r => r.LastPurchasePricePerKg).HasPrecision(18,4);
            });

            modelBuilder.Entity<ProductRawMaterial>(eb =>
            {
                eb.Property(x => x.QuantityKgPerPiece).HasPrecision(18,6);
            });

            modelBuilder.Entity<ProductionOrder>(eb =>
            {
                eb.Property(p => p.PricePerTonPET).HasPrecision(18,4);
                eb.Property(p => p.BagCost).HasPrecision(18,4);
                eb.Property(p => p.LaborCost).HasPrecision(18,4);
                eb.Property(p => p.ElectricityCost).HasPrecision(18,4);
                eb.Property(p => p.OtherExpenses).HasPrecision(18,4);
                eb.Property(p => p.WeightPerPieceKg).HasPrecision(18,6);
                eb.Property(p => p.CostPerPiece).HasPrecision(18,6);
                eb.Property(p => p.TotalRawMaterialCost).HasPrecision(18,4);
                eb.Property(p => p.TotalProductionCost).HasPrecision(18,4);
            });

            modelBuilder.Entity<FinishedStock>(eb =>
            {
                eb.Property(f => f.QuantityKg).HasPrecision(18,4);
                eb.Property(f => f.CostPerPiece).HasPrecision(18,6);
                eb.Property(f => f.TotalCost).HasPrecision(18,4);
            });

            modelBuilder.Entity<RawMaterialTransaction>(eb =>
            {
                eb.Property(t => t.QuantityKg).HasPrecision(18,4);
                eb.Property(t => t.UnitPricePerKg).HasPrecision(18,4);
                eb.Property(t => t.TotalCost).HasPrecision(18,4);
            });

            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        }
    }
}
