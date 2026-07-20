using PETFactoryERP.DAL;
using PETFactoryERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace PETFactoryERP.BLL
{
    public interface IRawMaterialService
    {
        Task<List<RawMaterial>> GetAllAsync();
        Task<RawMaterial?> GetByIdAsync(int id);
        Task AddAsync(RawMaterial rm);
        Task UpdateAsync(RawMaterial rm);
        Task AdjustStockAsync(int rawMaterialId, decimal qtyKg, string transactionType, decimal unitPricePerKg = 0m, int? referenceId = null, string? referenceType = null);
    }

    public class RawMaterialService : IRawMaterialService
    {
        private readonly AppDbContext _db;
        public RawMaterialService(AppDbContext db) { _db = db; }

        public async Task AddAsync(RawMaterial rm) { _db.RawMaterials.Add(rm); await _db.SaveChangesAsync(); }
        public async Task<List<RawMaterial>> GetAllAsync() => await _db.RawMaterials.AsNoTracking().ToListAsync();
        public async Task<RawMaterial?> GetByIdAsync(int id) => await _db.RawMaterials.FindAsync(id);
        public async Task UpdateAsync(RawMaterial rm) { _db.RawMaterials.Update(rm); await _db.SaveChangesAsync(); }

        public async Task AdjustStockAsync(int rawMaterialId, decimal qtyKg, string transactionType, decimal unitPricePerKg = 0m, int? referenceId = null, string? referenceType = null)
        {
            var rm = await _db.RawMaterials.FindAsync(rawMaterialId);
            if (rm == null) throw new Exception("Raw material not found");

            if (transactionType == "IN")
            {
                rm.StockQtyKg += qtyKg;
                rm.LastPurchasePricePerKg = unitPricePerKg > 0 ? unitPricePerKg : rm.LastPurchasePricePerKg;
            }
            else if (transactionType == "OUT")
            {
                if (rm.StockQtyKg < qtyKg) throw new Exception($"Insufficient stock for {rm.Name}");
                rm.StockQtyKg -= qtyKg;
            }
            _db.RawMaterialTransactions.Add(new RawMaterialTransaction
            {
                RawMaterialId = rawMaterialId,
                TransactionType = transactionType,
                QuantityKg = qtyKg,
                UnitPricePerKg = unitPricePerKg,
                TotalCost = unitPricePerKg * qtyKg,
                ReferenceId = referenceId,
                ReferenceType = referenceType
            });
            await _db.SaveChangesAsync();
        }
    }
}
