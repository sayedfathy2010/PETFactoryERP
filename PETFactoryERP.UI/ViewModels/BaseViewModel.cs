using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PETFactoryERP.UI.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected void Notify([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
