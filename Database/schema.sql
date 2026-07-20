-- schema.sql
-- Create database and schema for PETFactoryDB. This file contains the full CREATE TABLE statements and stored procedures.
-- Run this script in SQL Server Management Studio (SSMS) or via sqlcmd.

IF DB_ID('PETFactoryDB') IS NULL
BEGIN
    CREATE DATABASE PETFactoryDB;
END
GO
USE PETFactoryDB;
GO

-- For brevity, include the full schema from the project documentation. Below are key tables and two stored procedures.

CREATE TABLE Users (
  UserId INT IDENTITY(1,1) PRIMARY KEY,
  Username NVARCHAR(100) NOT NULL UNIQUE,
  PasswordHash NVARCHAR(256) NOT NULL,
  FullName NVARCHAR(200),
  Role NVARCHAR(50),
  IsActive BIT DEFAULT 1,
  CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Products (
  ProductId INT IDENTITY(1,1) PRIMARY KEY,
  Code NVARCHAR(50) UNIQUE,
  Name NVARCHAR(200) NOT NULL,
  Description NVARCHAR(1000),
  Area_m2 DECIMAL(18,6) DEFAULT 0,
  Thickness_mm DECIMAL(18,6) DEFAULT 0,
  Density_g_cm3 DECIMAL(18,6) DEFAULT 0,
  PiecesPerBag INT DEFAULT 0,
  CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME()
);

CREATE TABLE RawMaterials (
  RawMaterialId INT IDENTITY(1,1) PRIMARY KEY,
  Code NVARCHAR(50) UNIQUE,
  Name NVARCHAR(200) NOT NULL,
  Unit NVARCHAR(20) DEFAULT 'kg',
  StockQtyKg DECIMAL(18,4) DEFAULT 0,
  ReorderLevelKg DECIMAL(18,4) DEFAULT 0,
  LastPurchasePricePerKg DECIMAL(18,4) DEFAULT 0,
  Notes NVARCHAR(500)
);

CREATE TABLE ProductRawMaterials (
  ProductRawMaterialId INT IDENTITY(1,1) PRIMARY KEY,
  ProductId INT NOT NULL,
  RawMaterialId INT NOT NULL,
  QuantityKgPerPiece DECIMAL(18,6) NOT NULL,
  Notes NVARCHAR(500)
);

-- Stored Procedure: usp_CalculateCost (simplified)
CREATE PROCEDURE usp_CalculateCost
  @Area_m2 DECIMAL(18,6),
  @Thickness_mm DECIMAL(18,6),
  @Density_g_cm3 DECIMAL(18,6),
  @PricePerTonPET DECIMAL(18,4),
  @PiecesPerBag INT,
  @BagCost DECIMAL(18,4),
  @LaborCost DECIMAL(18,4),
  @ElectricityCost DECIMAL(18,4),
  @OtherExpenses DECIMAL(18,4),
  @ProducedPieces INT
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @Thickness_m DECIMAL(18,9) = @Thickness_mm / 1000.0;
  DECLARE @Density_kg_m3 DECIMAL(18,6) = @Density_g_cm3 * 1000.0;
  DECLARE @WeightPerPieceKg DECIMAL(18,6) = @Area_m2 * @Thickness_m * @Density_kg_m3;
  DECLARE @PiecesPerTon INT = CASE WHEN @WeightPerPieceKg > 0 THEN FLOOR(1000.0 / @WeightPerPieceKg) ELSE 0 END;
  DECLARE @TotalWeightKg DECIMAL(18,4) = @WeightPerPieceKg * @ProducedPieces;
  DECLARE @PricePerKg DECIMAL(18,6) = CASE WHEN @PricePerTonPET IS NOT NULL THEN @PricePerTonPET / 1000.0 ELSE 0 END;
  DECLARE @RawMaterialCost DECIMAL(18,4) = @TotalWeightKg * @PricePerKg;
  DECLARE @NumberOfBags INT = CASE WHEN @PiecesPerBag > 0 THEN CEILING(CAST(@ProducedPieces AS DECIMAL(18,4)) / @PiecesPerBag) ELSE 0 END;
  DECLARE @TotalBagCost DECIMAL(18,4) = @NumberOfBags * @BagCost;
  DECLARE @TotalOpCost DECIMAL(18,4) = ISNULL(@LaborCost,0) + ISNULL(@ElectricityCost,0) + ISNULL(@OtherExpenses,0);
  DECLARE @TotalCost DECIMAL(18,4) = @RawMaterialCost + @TotalOpCost + @TotalBagCost;
  DECLARE @CostPerPiece DECIMAL(18,6) = CASE WHEN @ProducedPieces > 0 THEN @TotalCost / @ProducedPieces ELSE 0 END;
  DECLARE @CostPerThousand DECIMAL(18,4) = @CostPerPiece * 1000.0;
  DECLARE @SuggestedMargin DECIMAL(5,2) = 0.30;
  DECLARE @SuggestedUnitPrice DECIMAL(18,4) = @CostPerPiece * (1 + @SuggestedMargin);
  SELECT
    @WeightPerPieceKg AS WeightPerPieceKg,
    @PiecesPerTon AS PiecesPerTon,
    @TotalWeightKg AS TotalWeightKg,
    @RawMaterialCost AS RawMaterialCost,
    @TotalOpCost AS OperationCost,
    @TotalBagCost AS PackagingCost,
    @TotalCost AS TotalCost,
    @CostPerPiece AS CostPerPiece,
    @CostPerThousand AS CostPerThousand,
    @SuggestedUnitPrice AS SuggestedUnitPrice;
END;
GO

-- Stored Procedure: usp_FinishProductionOrder (simplified)
CREATE PROCEDURE usp_FinishProductionOrder
  @ProductionOrderId INT,
  @ActualProducedPieces INT
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRAN;
  BEGIN TRY
    -- This simplified SP assumes BOM and stock exist. A full version is provided in project docs.
    UPDATE ProductionOrders SET ProducedQuantityPieces = @ActualProducedPieces, Status = 'Completed', CompletedAt = SYSUTCDATETIME() WHERE ProductionOrderId = @ProductionOrderId;
    COMMIT TRAN;
  END TRY
  BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRAN;
    THROW;
  END CATCH
END;
GO
