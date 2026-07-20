using PETFactoryERP.DAL;
using PETFactoryERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace PETFactoryERP.BLL
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        public AuthService(AppDbContext db) { _db = db; }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == username && u.IsActive);
            if (user == null) return null;

            bool ok = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!ok) return null;
            return user;
        }
    }
}
