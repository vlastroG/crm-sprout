using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class CompanyStatement : Entity, IValidatableObject {
        public CompanyStatement() : base() {
            Name = "default";
        }


        [Required]
        [MaxLength(32)]
        [DisplayName("Название")]
        public string Name { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
