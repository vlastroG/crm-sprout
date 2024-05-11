using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Consulting.Desktop.ViewModels {
    public abstract class ValidatableViewModel : BaseViewModel, IDataErrorInfo {
        protected ValidatableViewModel() : base() {

        }


        public virtual string Error {
            get {
                return GetType()
                    .GetProperties()
                    .Select(prop => this[prop.Name])
                    .FirstOrDefault(error => !string.IsNullOrWhiteSpace(error)) ?? string.Empty;
            }
        }

        public abstract string this[string columnName] {
            get;
        }

        private protected string GetStringPropertyError(PropertyInfo property) {
            if(property is null) { throw new ArgumentNullException(nameof(property)); }

            bool required = property.GetCustomAttribute<RequiredAttribute>() != null;
            int maxLength = property.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? int.MaxValue;
            string? propValue = property.GetValue(this)?.ToString();

            string error = string.Empty;
            if(required && string.IsNullOrWhiteSpace(propValue)) {
                error = "Поле не может быть пустым";
            } else if(propValue?.Length > maxLength) {
                error = $"Не более {maxLength} символов";
            } else if((propValue?.StartsWith(' ') ?? false) || (propValue?.EndsWith(' ') ?? false)) {
                error = "Поле не может начинаться или заканчиваться пробелом";
            }
            return error;
        }
    }
}
