# PETFactoryERP (انتاج-pet)

This repository contains the core of PETFactoryERP — a WPF (.NET 8) application in Arabic (RTL) for managing a small PET production factory.

What is included in this branch:
- Projects: PETFactoryERP.Models, PETFactoryERP.DAL, PETFactoryERP.BLL, PETFactoryERP.UI
- Core features: Models, DbContext, AuthService, basic Product & RawMaterial services, ProductionService (calls stored procedures), Login + Dashboard views (WPF)
- Database schema script: Database/schema.sql (create DB, tables, stored procedures)
- Data seeder that creates an admin user: username=admin password=123 (hashed). Change immediately after first login.

How to run locally:
1. Prerequisites: .NET 8 SDK, SQL Server, Visual Studio 2022/2023 or VS Code.
2. Execute Database/schema.sql in SQL Server to create the PETFactoryDB and required objects.
3. Edit connection string in PETFactoryERP.UI/App.xaml.cs if necessary (default: "Server=.;Database=PETFactoryDB;Trusted_Connection=True;")
4. Build and run the UI project:
   dotnet build
   dotnet run --project PETFactoryERP.UI

Default credentials (change immediately):
- username: admin
- password: 123

Notes:
- The project is scaffolded with MVVM pattern and DI via Microsoft.Extensions.DependencyInjection.
- Stored procedures usp_CalculateCost and usp_FinishProductionOrder are provided in the SQL script and used by ProductionService.

