using System.ComponentModel.DataAnnotations;

namespace Consulting.WebClient.Attributes {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IFormFileMaxLength : ValidationAttribute {
        private readonly int _maxLength;

        public IFormFileMaxLength(int maxLength) {
            _maxLength = maxLength;
        }

        public override bool IsValid(object? value) {
            var file = (IFormFile?) value;
            return file is null || file.Length <= _maxLength;
        }
    }
}
