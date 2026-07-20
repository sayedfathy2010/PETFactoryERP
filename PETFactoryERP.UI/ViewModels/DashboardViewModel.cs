using PETFactoryERP.Models;

namespace PETFactoryERP.UI.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public DashboardViewModel(User user)
        {
            WelcomeText = $"مرحباً، {user.FullName ?? user.Username}";
            TotalRawMaterialsKg = 0;
            TotalFinishedPieces = 0;
            InProgressOrders = 0;
        }
        public string WelcomeText { get; set; }
        public decimal TotalRawMaterialsKg { get; set; }
        public int TotalFinishedPieces { get; set; }
        public int InProgressOrders { get; set; }
    }
}
