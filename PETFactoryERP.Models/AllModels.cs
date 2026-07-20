// Models/AllModels.cs
using System;

namespace PETFactoryERP.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Customer { public int CustomerId { get; set; } public string? Code { get; set; } public string Name { get; set; } = null!; public string? Address { get; set; } public string? Phone { get; set; } public string? Email { get; set; } public string? TaxNumber { get; set; } public string? Notes { get; set; } }

    public class Supplier { public int SupplierId { get; set; } public string? Code { get; set; } public string Name { get; set; } = null!; public string? Address { get; set; } public string? Phone { get; set; } public string? Email { get; set; } public string? TaxNumber { get; set; } public string? Notes { get; set; } }

    public class Product { public int ProductId { get; set; } public string? Code { get; set; } public string Name { get; set; } = null!; public string? Description { get; set; } public decimal Area_m2 { get; set; } public decimal Thickness_mm { get; set; } public decimal Density_g_cm3 { get; set; } public int PiecesPerBag { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }

    public class RawMaterial { public int RawMaterialId { get; set; } public string? Code { get; set; } public string Name { get; set; } = null!; public string Unit { get; set; } = "kg"; public decimal StockQtyKg { get; set; } public decimal ReorderLevelKg { get; set; } public decimal LastPurchasePricePerKg { get; set; } public string? Notes { get; set; } }

    public class ProductionOrder { public int ProductionOrderId { get; set; } public string OrderNumber { get; set; } = null!; public int ProductId { get; set; } public int TargetQuantityPieces { get; set; } public int ProducedQuantityPieces { get; set; } public string Status { get; set; } = "New"; public DateTime? StartedAt { get; set; } public DateTime? CompletedAt { get; set; } public decimal? PricePerTonPET { get; set; } public decimal BagCost { get; set; } public decimal LaborCost { get; set; } public decimal ElectricityCost { get; set; } public decimal OtherExpenses { get; set; } public decimal WeightPerPieceKg { get; set; } public decimal CostPerPiece { get; set; } public decimal TotalRawMaterialCost { get; set; } public decimal TotalProductionCost { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }

    public class FinishedStock { public int FinishedStockId { get; set; } public int ProductId { get; set; } public string? BatchNumber { get; set; } public int QuantityPieces { get; set; } public decimal QuantityKg { get; set; } public decimal CostPerPiece { get; set; } public decimal TotalCost { get; set; } public DateTime ReceivedAt { get; set; } = DateTime.UtcNow; public int? SourceProductionOrderId { get; set; } }

    public class RawMaterialTransaction { public int RMTransactionId { get; set; } public int RawMaterialId { get; set; } public string TransactionType { get; set; } = null!; public decimal QuantityKg { get; set; } public decimal? UnitPricePerKg { get; set; } public decimal? TotalCost { get; set; } public string? ReferenceType { get; set; } public int? ReferenceId { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }

    public class SalesInvoice { public int SalesInvoiceId { get; set; } public string? InvoiceNumber { get; set; } public DateTime InvoiceDate { get; set; } = DateTime.UtcNow; public int? CustomerId { get; set; } public decimal TotalAmount { get; set; } public decimal PaidAmount { get; set; } public string? Notes { get; set; } }

    public class SalesInvoiceDetail { public int SalesInvoiceDetailId { get; set; } public int SalesInvoiceId { get; set; } public int ProductId { get; set; } public int Quantity { get; set; } public decimal UnitPrice { get; set; } public decimal LineTotal { get; set; } }

    public class PurchaseInvoice { public int PurchaseInvoiceId { get; set; } public string? InvoiceNumber { get; set; } public DateTime InvoiceDate { get; set; } = DateTime.UtcNow; public int? SupplierId { get; set; } public decimal TotalAmount { get; set; } public string? Notes { get; set; } }

    public class PurchaseInvoiceDetail { public int PurchaseInvoiceDetailId { get; set; } public int PurchaseInvoiceId { get; set; } public int RawMaterialId { get; set; } public decimal QuantityKg { get; set; } public decimal UnitPricePerKg { get; set; } public decimal LineTotal { get; set; } }

    public class DailyWage { public int WageId { get; set; } public DateTime DateWorked { get; set; } public int? ProductionOrderId { get; set; } public string? EmployeeName { get; set; } public decimal Hours { get; set; } public decimal Amount { get; set; } public string? Notes { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }

    public class Expense { public int ExpenseId { get; set; } public DateTime ExpenseDate { get; set; } public int? ProductionOrderId { get; set; } public string? Type { get; set; } public decimal Amount { get; set; } public string? Description { get; set; } public DateTime CreatedAt { get; set; } = DateTime.UtcNow; }

    public class CashTransaction { public int CashTransactionId { get; set; } public DateTime TransactionDate { get; set; } = DateTime.UtcNow; public string TransactionType { get; set; } = null!; public decimal Amount { get; set; } public string? ReferenceType { get; set; } public int? ReferenceId { get; set; } public string? Description { get; set; } }

    public class ProductRawMaterial { public int ProductRawMaterialId { get; set; } public int ProductId { get; set; } public int RawMaterialId { get; set; } public decimal QuantityKgPerPiece { get; set; } public string? Notes { get; set; } }
}
