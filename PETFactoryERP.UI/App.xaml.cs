using Microsoft.Extensions.DependencyInjection;
using PETFactoryERP.BLL;
using PETFactoryERP.DAL;
using PETFactoryERP.UI.ViewModels;
using System;
using System.Windows;

namespace PETFactoryERP.UI
{
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            string connectionString = "Server=.;Database=PETFactoryDB;Trusted_Connection=True;";
            services.AddSingleton(sp => new AppDbContext(connectionString));
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IProductionService>(sp => new ProductionService(connectionString));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRawMaterialService, RawMaterialService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();

            _serviceProvider = services.BuildServiceProvider();

            // Ensure DB seeded (try-catch to avoid errors if DB not created yet)
            try
            {
                var db = _serviceProvider.GetRequiredService<AppDbContext>();
                DataSeeder.Seed(db);
            }
            catch { /* ignore if DB not ready */ }

            var loginWindow = new Views.LoginView();
            loginWindow.DataContext = _serviceProvider.GetRequiredService<LoginViewModel>();
            loginWindow.Show();
        }
    }
}
