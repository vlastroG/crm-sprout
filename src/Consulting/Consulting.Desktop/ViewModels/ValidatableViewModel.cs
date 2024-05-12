using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
            EmailAddressAttribute? emailAttribute = property.GetCustomAttribute<EmailAddressAttribute>();
            string? propValue = property.GetValue(this)?.ToString();

            string error = string.Empty;
            if(required && string.IsNullOrWhiteSpace(propValue)) {
                error = "Поле не может быть пустым";
            } else if(propValue?.Length > maxLength) {
                error = $"Не более {maxLength} символов";
            } else if((propValue?.StartsWith(' ') ?? false) || (propValue?.EndsWith(' ') ?? false)) {
                error = "Поле не может начинаться или заканчиваться пробелом";
            } else if(emailAttribute is not null) {
                bool valid = emailAttribute.IsValid(propValue);
                error = valid ? string.Empty : "Некорректный email";
            }
            return error;
        }

        private protected string GetFileInfoPropertyError(PropertyInfo property, int maxBytesLength) {
            if(property is null) { throw new ArgumentNullException(nameof(property)); }

            FileInfo? file = property.GetValue(this) as FileInfo;
            string error = string.Empty;
            if(file?.Length > maxBytesLength) {
                error = $"Не более {maxBytesLength / 1024} КБ";
            }
            return error;
        }

        private protected PropertyInfo GetProperty(string name) {
            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            return GetType().GetProperties().First(prop => prop.Name == name);
        }
    }
}
