using PETFactoryERP.Models;
using System;
using System.Linq;

namespace PETFactoryERP.DAL
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // تأكد من وجود جدول المستخدمين — إن لم يوجد سيُرمى استثناء قبل الاستدعاء
            if (!db.Users.Any())
            {
                var pwd = "123"; // الافتراضي — يمكن تغييره لاحقاً
                var hash = BCrypt.Net.BCrypt.HashPassword(pwd);
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = hash,
                    FullName = "Administrator",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                db.Users.Add(admin);
                db.SaveChanges();
            }
        }
    }
}
