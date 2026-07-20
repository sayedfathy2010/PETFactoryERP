using System.Windows;
using System.Windows.Controls;
using PETFactoryERP.UI.ViewModels;

namespace PETFactoryERP.UI.Views
{
    public partial class LoginView : Window
    {
        public LoginView() { InitializeComponent(); }
        private void PwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel vm && sender is PasswordBox pb)
            {
                vm.Password = pb.Password;
            }
        }
    }
}
