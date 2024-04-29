using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public abstract class Entity {
        protected Entity() { }


        [Key]
        public int Id { get; set; }


        private protected IEnumerable<ValidationResult> ValidateTextProperties() {
            var properties = GetType()
                .GetProperties()
                .Where(p => p.GetAccessors().Any(a => a.IsPublic) && p.PropertyType.Equals(typeof(string)));
            foreach(var field in properties) {
                string str = field.GetValue(this) as string ?? string.Empty;
                if(string.IsNullOrWhiteSpace(str)) {
                    yield return new ValidationResult("Value cannot be empty", new[] { field.Name });
                } else if(str.Length != str.Trim().Length) {
                    yield return new ValidationResult("Value cannot start or end with space", new[] { field.Name });
                }
            }
        }
    }
}
