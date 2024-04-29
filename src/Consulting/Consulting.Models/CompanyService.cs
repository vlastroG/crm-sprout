using System.ComponentModel.DataAnnotations;

namespace Consulting.Models {
    public class CompanyService : Entity, IValidatableObject {
        public CompanyService() : base() {
            Name = "default";
            Description = "default description";
        }


        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var textErrors = ValidateTextProperties();
            foreach(var error in textErrors) {
                yield return error;
            }
        }
    }
}
