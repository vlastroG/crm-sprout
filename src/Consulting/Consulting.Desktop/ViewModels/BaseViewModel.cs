using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Consulting.Desktop.ViewModels {
    public abstract class BaseViewModel : INotifyPropertyChanged {
        protected BaseViewModel() { }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Set value to property of ViewModel if it has different value and raise PropertyChanged event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">Backing field</param>
        /// <param name="value">New value</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>If value has changed - True, else - False</returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
            if(EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
