using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace PETFactoryERP.BLL
{
    public class CostCalculationResult
    {
        public decimal WeightPerPieceKg { get; set; }
        public int PiecesPerTon { get; set; }
        public decimal TotalWeightKg { get; set; }
        public decimal RawMaterialCost { get; set; }
        public decimal OperationCost { get; set; }
        public decimal PackagingCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal CostPerPiece { get; set; }
        public decimal CostPerThousand { get; set; }
        public decimal SuggestedUnitPrice { get; set; }
    }

    public interface IProductionService
    {
        Task FinishProductionOrderAsync(int productionOrderId, int actualProducedPieces);
        Task<CostCalculationResult> CalculateCostAsync(decimal area_m2, decimal thickness_mm, decimal density_g_cm3, decimal pricePerTonPET, int piecesPerBag, decimal bagCost, decimal laborCost, decimal electricityCost, decimal otherExpenses, int producedPieces);
    }

    public class ProductionService : IProductionService
    {
        private readonly string _connectionString;
        public ProductionService(string connectionString) { _connectionString = connectionString; }

        public async Task FinishProductionOrderAsync(int productionOrderId, int actualProducedPieces)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_FinishProductionOrder", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@ProductionOrderId", productionOrderId);
            cmd.Parameters.AddWithValue("@ActualProducedPieces", actualProducedPieces);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<CostCalculationResult> CalculateCostAsync(decimal area_m2, decimal thickness_mm, decimal density_g_cm3, decimal pricePerTonPET, int piecesPerBag, decimal bagCost, decimal laborCost, decimal electricityCost, decimal otherExpenses, int producedPieces)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_CalculateCost", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Area_m2", area_m2);
            cmd.Parameters.AddWithValue("@Thickness_mm", thickness_mm);
            cmd.Parameters.AddWithValue("@Density_g_cm3", density_g_cm3);
            cmd.Parameters.AddWithValue("@PricePerTonPET", pricePerTonPET);
            cmd.Parameters.AddWithValue("@PiecesPerBag", piecesPerBag);
            cmd.Parameters.AddWithValue("@BagCost", bagCost);
            cmd.Parameters.AddWithValue("@LaborCost", laborCost);
            cmd.Parameters.AddWithValue("@ElectricityCost", electricityCost);
            cmd.Parameters.AddWithValue("@OtherExpenses", otherExpenses);
            cmd.Parameters.AddWithValue("@ProducedPieces", producedPieces);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var res = new CostCalculationResult();
            if (await reader.ReadAsync())
            {
                res.WeightPerPieceKg = reader.GetDecimal(reader.GetOrdinal("WeightPerPieceKg"));
                res.PiecesPerTon = reader.GetInt32(reader.GetOrdinal("PiecesPerTon"));
                res.TotalWeightKg = reader.GetDecimal(reader.GetOrdinal("TotalWeightKg"));
                res.RawMaterialCost = reader.GetDecimal(reader.GetOrdinal("RawMaterialCost"));
                res.OperationCost = reader.GetDecimal(reader.GetOrdinal("OperationCost"));
                res.PackagingCost = reader.GetDecimal(reader.GetOrdinal("PackagingCost"));
                res.TotalCost = reader.GetDecimal(reader.GetOrdinal("TotalCost"));
                res.CostPerPiece = reader.GetDecimal(reader.GetOrdinal("CostPerPiece"));
                res.CostPerThousand = reader.GetDecimal(reader.GetOrdinal("CostPerThousand"));
                res.SuggestedUnitPrice = reader.GetDecimal(reader.GetOrdinal("SuggestedUnitPrice"));
            }
            return res;
        }
    }
}
