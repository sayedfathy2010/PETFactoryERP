using System;
using System.Data;
using System.Data.SqlClient;
using PETFactoryERP.Models;

namespace PETFactoryERP.DAL
{
    /// <summary>
    /// طبقة الوصول لبيانات الموردين
    /// </summary>
    public class SupplierRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public SupplierRepository(string connectionString)
        {
            _dbConnection = new DatabaseConnection(connectionString);
        }

        /// <summary>
        /// الحصول على جميع الموردين
        /// </summary>
        public DataTable GetAllSuppliers()
        {
            string query = @"SELECT SupplierID, SupplierName, ContactPerson, Email, PhoneNumber, City, 
                            Rating, IsActive, CreatedDate
                            FROM Suppliers WHERE IsActive = 1 ORDER BY SupplierName";
            return _dbConnection.ExecuteQuery(query);
        }

        /// <summary>
        /// إضافة مورد جديد
        /// </summary>
        public bool AddSupplier(Supplier supplier)
        {
            try
            {
                string query = $@"INSERT INTO Suppliers (SupplierName, ContactPerson, Email, PhoneNumber, Address, 
                                City, Country, TaxID, PaymentTerms, Rating, IsActive, CreatedDate)
                                VALUES ('{supplier.SupplierName}', '{supplier.ContactPerson}', '{supplier.Email}', 
                                '{supplier.PhoneNumber}', '{supplier.Address}', '{supplier.City}', 
                                '{supplier.Country}', '{supplier.TaxID}', '{supplier.PaymentTerms}', 
                                {supplier.Rating}, 1, GETDATE())";
                return _dbConnection.ExecuteCommand(query) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إضافة مورد: {ex.Message}");
            }
        }

        /// <summary>
        /// تحديث بيانات المورد
        /// </summary>
        public bool UpdateSupplier(Supplier supplier)
        {
            try
            {
                string query = $@"UPDATE Suppliers SET 
                                SupplierName = '{supplier.SupplierName}',
                                ContactPerson = '{supplier.ContactPerson}',
                                Email = '{supplier.Email}',
                                PhoneNumber = '{supplier.PhoneNumber}',
                                Address = '{supplier.Address}',
                                City = '{supplier.City}',
                                PaymentTerms = '{supplier.PaymentTerms}',
                                Rating = {supplier.Rating},
                                UpdatedDate = GETDATE()
                                WHERE SupplierID = {supplier.SupplierID}";
                return _dbConnection.ExecuteCommand(query) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في تحديث المورد: {ex.Message}");
            }
        }

        /// <summary>
        /// حذف مورد
        /// </summary>
        public bool DeleteSupplier(int supplierId)
        {
            try
            {
                string query = $"UPDATE Suppliers SET IsActive = 0 WHERE SupplierID = {supplierId}";
                return _dbConnection.ExecuteCommand(query) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حذف المورد: {ex.Message}");
            }
        }
    }
}
