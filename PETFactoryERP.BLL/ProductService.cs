using PETFactoryERP.DAL;
using PETFactoryERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PETFactoryERP.BLL
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product p);
        Task UpdateAsync(Product p);
        Task DeleteAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        public ProductService(AppDbContext db) { _db = db; }

        public async Task AddAsync(Product p) { _db.Products.Add(p); await _db.SaveChangesAsync(); }
        public async Task DeleteAsync(int id) { var e = await _db.Products.FindAsync(id); if (e != null) { _db.Products.Remove(e); await _db.SaveChangesAsync(); } }
        public async Task<List<Product>> GetAllAsync() => await _db.Products.AsNoTracking().ToListAsync();
        public async Task<Product?> GetByIdAsync(int id) => await _db.Products.FindAsync(id);
        public async Task UpdateAsync(Product p) { _db.Products.Update(p); await _db.SaveChangesAsync(); }
    }
}
