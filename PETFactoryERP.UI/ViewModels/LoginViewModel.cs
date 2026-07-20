using PETFactoryERP.BLL;
using PETFactoryERP.Models;
using PETFactoryERP.UI.Commands;
using System.Threading.Tasks;
using System.Windows;

namespace PETFactoryERP.UI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _auth;
        private readonly AppDbContext _db; // used for seeding check if needed

        public LoginViewModel(IAuthService auth, AppDbContext db)
        {
            _auth = auth;
            _db = db;
            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => !IsBusy);
        }

        private string _username = "";
        public string Username { get => _username; set => Set(ref _username, value); }

        private string _password = "";
        public string Password { get => _password; set => Set(ref _password, value); }

        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { Set(ref _isBusy, value); ((RelayCommand)LoginCommand).RaiseCanExecuteChanged(); } }

        public RelayCommand LoginCommand { get; }

        private async Task LoginAsync()
        {
            IsBusy = true;
            try
            {
                var user = await _auth.AuthenticateAsync(Username.Trim(), Password);
                if (user == null)
                {
                    MessageBox.Show("خطأ في اسم المستخدم أو كلمة المرور", "فشل", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Open Dashboard window
                var dashboard = new Views.DashboardView();
                dashboard.DataContext = new DashboardViewModel(user);
                dashboard.Show();

                // Close Login window
                Application.Current.Windows[0]?.Close();
            }
            finally { IsBusy = false; }
        }
    }
}
